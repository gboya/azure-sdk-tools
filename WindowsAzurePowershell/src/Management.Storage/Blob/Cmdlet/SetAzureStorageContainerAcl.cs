﻿﻿// ----------------------------------------------------------------------------------
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

namespace Microsoft.WindowsAzure.Management.Storage.Cmdlet
{
    using Microsoft.WindowsAzure.Management.Storage.Common;
    using Microsoft.WindowsAzure.Management.Storage.Model.Contract;
    using Microsoft.WindowsAzure.Management.Storage.Model.ResourceModel;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Globalization;
    using System.Management.Automation;
    using System.Security.Permissions;

    /// <summary>
    /// set access level for specified container
    /// </summary>
    [Cmdlet(VerbsCommon.Set, StorageNouns.ContainerAcl),
        OutputType(typeof(AzureStorageContainer))]
    public class SetAzureStorageContainerAclCommand : StorageCloudBlobCmdletBase
    {
        [Alias("N", "Container")]
        [Parameter(Position = 0, Mandatory = true, HelpMessage = "Container Name",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        [Alias("PublicAccess")]
        [Parameter(Position = 1, Mandatory = true,
            HelpMessage = "Permission string Off/Blob/Container")]
        [ValidateSet(StorageNouns.ContainerAclOff, StorageNouns.ContainerAclBlob, StorageNouns.ContainerAclContainer, IgnoreCase = true)]
        [ValidateNotNullOrEmpty]
        public string Permission
        {
            get { return accessLevel; }
            set { accessLevel = value; }
        }
        private string accessLevel = StorageNouns.ContainerAclOff;

        [Parameter(Mandatory = false, HelpMessage = "Display Container Information")]
        public SwitchParameter PassThru { get; set; }

        /// <summary>
        /// Initializes a new instance of the SetAzureStorageContainerAclCommand class.
        /// </summary>
        public SetAzureStorageContainerAclCommand()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SetAzureStorageContainerAclCommand class.
        /// </summary>
        /// <param name="channel">IStorageBlobManagement channel</param>
        public SetAzureStorageContainerAclCommand(IStorageBlobManagement channel)
        {
            Channel = channel;
        }

        /// <summary>
        /// set the access level of specified container
        /// </summary>
        /// <param name="name">container name</param>
        /// <param name="accessLevel">access level in ("off", "blob", "container")</param>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        internal void SetContainerAcl(string name, string accessLevel)
        {
            if (!NameUtil.IsValidContainerName(name))
            {
                throw new ArgumentException(String.Format(Resources.InvalidContainerName, name));
            }

            BlobContainerPermissions permissions = new BlobContainerPermissions();

            switch (CultureInfo.CurrentCulture.TextInfo.ToTitleCase(accessLevel))
            {
                case StorageNouns.ContainerAclOff:
                    permissions.PublicAccess = BlobContainerPublicAccessType.Off;
                    break;
                case StorageNouns.ContainerAclBlob:
                    permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                    break;
                case StorageNouns.ContainerAclContainer:
                    permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                    break;
                default:
                    throw new ArgumentException(Resources.OnlyOnePermissionForContainer);
            }

            BlobRequestOptions requestOptions = null;
            AccessCondition accessCondition = null;

            CloudBlobContainer container = Channel.GetContainerReference(name);

            if (!Channel.DoesContainerExist(container, requestOptions, OperationContext))
            {
                throw new ResourceNotFoundException(String.Format(Resources.ContainerNotFound, name));
            }

            Channel.SetContainerPermissions(container, permissions, accessCondition, requestOptions, OperationContext);
            AzureStorageContainer azureContainer = new AzureStorageContainer(container, permissions);

            if (PassThru)
            {
                WriteObjectWithStorageContext(azureContainer);
            }
        }

        /// <summary>
        /// execute command
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public override void ExecuteCmdlet()
        {
            SetContainerAcl(Name, accessLevel);
        }
    }
}