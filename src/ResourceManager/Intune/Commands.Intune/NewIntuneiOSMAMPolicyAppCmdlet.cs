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

namespace Microsoft.Azure.Commands.Intune
{
    using System;
    using System.Management.Automation;
    using RestClient;
    using RestClient.Models;

    /// <summary>
    /// A cmdlet to link an app to iOS Intune MAM policy Azure resource.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AzureRmIntuneiOSMAMPolicyApp", SupportsShouldProcess = true), OutputType(typeof(PSObject))]
    public sealed class NewIntuneiOSMAMPolicyAppCmdlet : IntuneBaseCmdlet
    {
        /// <summary>
        /// Gets or sets the policy id
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "The iOS policy Id.")]
        [ValidateNotNullOrEmpty]
        public string PolicyId { get; set; }

        /// <summary>
        /// Gets or sets the App id
        /// </summary>
        [Parameter(Mandatory = true, HelpMessage = "The iOS App Name to link to.")]
        [ValidateNotNullOrEmpty]
        public string AppName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, HelpMessage = "Don't ask for confirmation.")]
        public SwitchParameter Force { get; set; }
        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        protected override void ProcessRecord()
        {
            Action action = () =>
            {
                this.ConfirmAction(
                    this.Force,
                    "Are you sure you want to add App with name:" + this.AppName + " to iOS policy with id:" + this.PolicyId,
                    "Link the app with iOS policy resource...",
                    this.PolicyId,
                    () =>
                    {
                        this.IntuneClient.AddAppForiOSMAMPolicy(this.AsuHostName, this.PolicyId, this.AppName, PrepareMAMPolicyAppIdGroupIdPayload());
                        this.WriteObject("Operation completed successfully");
                    });
            };

            base.SafeExecutor(action);
        }

        private MAMPolicyAppIdOrGroupIdPayload PrepareMAMPolicyAppIdGroupIdPayload()
        {
            string appUri = string.Format("https://{0}/providers/Microsoft.Intune/locations/{1}/apps/{2}", this.IntuneClient.BaseUri.Host, this.AsuHostName, this.AppName);
            var appIdPayload = new MAMPolicyAppIdOrGroupIdPayload();
            appIdPayload.Properties = new MAMPolicyAppOrGroupIdProperties()
            {
                Url = appUri
            };

            return appIdPayload;
        }
    }
}