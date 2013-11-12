﻿namespace Rackspace.VisualStudio.CloudExplorer.Dns
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.VSDesigner.ServerExplorer;
    using net.openstack.Providers.Rackspace;
    using net.openstack.Providers.Rackspace.Objects.Dns;
    using LocalizableProperties = Microsoft.VisualStudio.Shell.LocalizableProperties;

    public class CloudDomainNode : AsyncNode
    {
        private readonly CloudDnsProvider _provider;
        private readonly DnsDomain _domain;

        public CloudDomainNode(CloudDnsProvider provider, DnsDomain domain)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (domain == null)
                throw new ArgumentNullException("domain");

            _provider = provider;
            _domain = domain;
        }

        public override int CompareUnique(Node node)
        {
            return Label.CompareTo(node.Label);
        }

        protected override Task<Node[]> CreateChildrenAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(RackspaceProductsNode.EmptyChildren);
        }

        public override Image Icon
        {
            get
            {
                return RackspaceProductsNode.EmptyIcon;
            }
        }

        protected override string DisplayText
        {
            get
            {
                return _domain.Name;
            }
        }

        public override bool CanDeleteNode()
        {
            return true;
        }

        public override bool ConfirmDeletingNode()
        {
            throw new NotImplementedException();
        }

        public override object GetBrowseComponent()
        {
            return new DomainProperties(_provider, _domain);
        }

        private class DomainProperties : LocalizableProperties, ICustomTypeDescriptor
        {
            private readonly CloudDnsProvider _provider;
            private readonly DnsDomain _domain;

            public DomainProperties(CloudDnsProvider provider, DnsDomain domain)
            {
                if (provider == null)
                    throw new ArgumentNullException("provider");
                if (domain == null)
                    throw new ArgumentNullException("domain");

                _provider = provider;
                _domain = domain;
            }

            [DisplayName("ID")]
            public DomainId Id
            {
                get
                {
                    return _domain.Id;
                }
            }

            [DisplayName("Name")]
            public string Name
            {
                get
                {
                    return _domain.Name;
                }
            }

            [DisplayName("Account ID")]
            public string AccountId
            {
                get
                {
                    return _domain.AccountId;
                }
            }

            [DisplayName("Email Address")]
            public string EmailAddress
            {
                get
                {
                    return _domain.EmailAddress;
                }
            }

            [DisplayName("Comment")]
            public string Comment
            {
                get
                {
                    return _domain.Comment;
                }
            }

            [DisplayName("Time-To-Live")]
            public TimeSpan? TimeToLive
            {
                get
                {
                    return _domain.TimeToLive;
                }
            }

            [DisplayName("Created")]
            public DateTime? Created
            {
                get
                {
                    if (_domain.Created == null)
                        return null;

                    return _domain.Created.Value.ToLocalTime().DateTime;
                }
            }

            [DisplayName("Last Modified")]
            public DateTime? Updated
            {
                get
                {
                    if (_domain.Updated == null)
                        return null;

                    return _domain.Updated.Value.ToLocalTime().DateTime;
                }
            }

            public override string GetClassName()
            {
                return "Domain Properties";
            }

            string ICustomTypeDescriptor.GetComponentName()
            {
                return "Cloud DNS";
            }
        }
    }
}
