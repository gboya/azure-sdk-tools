﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.ServiceManagement.Extensions
{
    using Commands.Utilities.Common;

    public class DiagnosticExtensionContext : ManagementOperationContext
    {
        public ExtensionRole Role { get; set; }
        public string Extension { get; set; }
        public string ProviderNameSpace { get; set; }
        public string Id { get; set; }
        public string StorageAccountName { get; set; }
        public string WadCfg { get; set; }
    }
}