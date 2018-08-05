// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;

namespace RoutingConstraints.Server.Models
{
    public class CustomerVersion2
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public AddressVersion2 Address { get; set; }
    }
}