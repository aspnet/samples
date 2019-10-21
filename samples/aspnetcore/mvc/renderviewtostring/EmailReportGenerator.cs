// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Mvc.RenderViewToString
{
    public class EmailReportGenerator
    {
        public string GenerateReport(int userData1, int userData2)
        {
            return "A user specific report based on values " + userData1 + " and " + userData2;
        }
    }
}
