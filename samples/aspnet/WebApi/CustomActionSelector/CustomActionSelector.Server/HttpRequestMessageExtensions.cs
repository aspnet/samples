// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CustomActionSelector.Server
{
    internal static class HttpRequestMessageExtensions
    {
        public static bool IsCurrentUserBetaTester(this HttpRequestMessage request)
        {
            IEnumerable<string> headers;
            if (request.Headers.TryGetValues("beta-tester", out headers))
            {
                bool isBetaTester;
                if (headers.Count() == 1 && Boolean.TryParse(headers.First(), out isBetaTester))
                {
                    return isBetaTester;
                }
            }

            // No clear answer, assume this user is not in the beta program
            return false;
        }
    }
}