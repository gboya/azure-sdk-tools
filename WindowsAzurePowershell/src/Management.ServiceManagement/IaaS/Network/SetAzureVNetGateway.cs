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

namespace Microsoft.WindowsAzure.Management.ServiceManagement.IaaS
{
    using System.Management.Automation;
    using Service.Gateway;

    [Cmdlet(VerbsCommon.Set, "AzureVNetGateway", DefaultParameterSetName = "Connect")]
    public class SetAzureVNetGatewayCommand : GatewayCmdletBase
    {
        public SetAzureVNetGatewayCommand()
        {
        }

        public SetAzureVNetGatewayCommand(IGatewayServiceManagement channel)
        {
            Channel = channel;
        }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Connect", HelpMessage = "Connect to Gateway")]
        public SwitchParameter Connect
        {
            get;
            set;
        }

        [Parameter(Position = 0, Mandatory = true, ParameterSetName = "Disconnect", HelpMessage = "Disconnect from Gateway")]
        public SwitchParameter Disconnect
        {
            get;
            set;
        }

        [Parameter(Position = 1, Mandatory = true, HelpMessage = "Virtual network name.")]
        public string VNetName
        {
            get;
            set;
        }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = "Local Site network name.")]
        public string LocalNetworkSiteName
        {
            get;
            set;
        }

        protected override void OnProcessRecord()
        {
            var uc = new UpdateConnection
            {
                Operation = this.Connect.IsPresent ? UpdateConnectionOperation.Connect : UpdateConnectionOperation.Disconnect
            };

            this.ExecuteClientActionInOCS(null, this.CommandRuntime.ToString(), s => this.Channel.UpdateVirtualNetworkGatewayConnection(s, this.VNetName, this.LocalNetworkSiteName, uc), this.WaitForGatewayOperation);
        }
    }
}
