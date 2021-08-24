using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace JSComponentGeneration.Build.Common
{
    internal class RazorComponentReader
    {
        public static IEnumerable<string> ReadWithAttributeFromAssembly(string assemblyFilePath, string requiredAttributeName)
        {
            using var fileStream = new FileStream(assemblyFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fileStream);

            var metadataReader = peReader.GetMetadataReader();

            foreach (var typeDefinitionHandle in metadataReader.TypeDefinitions)
            {
                if (typeDefinitionHandle.IsNil)
                {
                    continue;
                }

                var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);

                if (!TypeDefinitionHasAttribute(metadataReader, in typeDefinition, requiredAttributeName))
                {
                    continue;
                }

                var typeName = metadataReader.GetString(typeDefinition.Name);
                yield return typeName;
            }
        }

        private static bool TypeDefinitionHasAttribute(MetadataReader metadataReader, in TypeDefinition typeDefinition, string attributeName)
        {
            foreach (var customAttributeHandle in typeDefinition.GetCustomAttributes())
            {
                if (!TryGetCustomAttributeType(metadataReader, in customAttributeHandle, out var attributeTypeHandle))
                {
                    continue;
                }
                
                if (!TryGetTypeName(metadataReader, in attributeTypeHandle, out var attributeTypeName))
                {
                    continue;
                }

                if (attributeTypeName != attributeName)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        private static bool TryGetCustomAttributeType(MetadataReader metadataReader, in CustomAttributeHandle customAttributeHandle, out EntityHandle typeHandle)
        {
            if (customAttributeHandle.IsNil)
            {
                typeHandle = default;
                return false;
            }

            var customAttribute = metadataReader.GetCustomAttribute(customAttributeHandle);
            var attributeConstructor = customAttribute.Constructor;

            if (attributeConstructor.IsNil)
            {
                typeHandle = default;
                return false;
            }

            switch (attributeConstructor.Kind)
            {
                case HandleKind.MethodDefinition:
                    typeHandle = metadataReader.GetMethodDefinition((MethodDefinitionHandle)attributeConstructor).GetDeclaringType();
                    return true;
                case HandleKind.MemberReference:
                    typeHandle = metadataReader.GetMemberReference((MemberReferenceHandle)attributeConstructor).Parent;
                    return true;
                default:
                    typeHandle = default;
                    return false;
            }
        }

        private static bool TryGetTypeName(MetadataReader metadataReader, in EntityHandle entityHandle, out string typeName)
        {
            if (entityHandle.IsNil)
            {
                typeName = string.Empty;
                return false;
            }

            StringHandle typeNameHandle;

            switch (entityHandle.Kind)
            {
                case HandleKind.TypeReference:
                    typeNameHandle = metadataReader.GetTypeReference((TypeReferenceHandle)entityHandle).Name;
                    break;
                case HandleKind.TypeDefinition:
                    typeNameHandle = metadataReader.GetTypeDefinition((TypeDefinitionHandle)entityHandle).Name;
                    break;
                default:
                    typeName = string.Empty;
                    return false;
            }

            if (typeNameHandle.IsNil)
            {
                typeName = string.Empty;
                return false;
            }

            typeName = metadataReader.GetString(typeNameHandle);
            return true;
        }
    }
}
