using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint;

namespace Akumina.ListDefinition.Provision.ManagedMetadataSolution
{
    public static class AkuminaTaxonomyExtensions
    {
        /// <summary>
        /// Finds a taxonomy group by its name.
        /// </summary>
        public static Group GetByName(this GroupCollection groupCollection, string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
            {
                throw new ArgumentException("Taxonomy group name cannot be empty", "name");
            }
            foreach (var group in groupCollection)
            {
                if (group.Name == groupName)
                {
                    return group;
                }
            }
            throw new ArgumentOutOfRangeException("groupName", groupName, "Could not find the taxonomy group");
        }

        /// <summary>
        /// Gets the default term store associated with the specified site collection.
        /// </summary>
        public static TermStore GetDefaultTermStore(SPSite site)
        {
            TermStore termStore = null;
            if (site != null)
            {
                //
                // Create an entry point for the taxonomy API
                //
                TaxonomySession taxonomySession = new TaxonomySession(site);
                //
                // Get hold of the default term store
                //
                termStore = taxonomySession.DefaultSiteCollectionTermStore;
                if (termStore == null)
                {
                    //
                    // In exceptional cases when default term store might be null, 
                    // try to get the first term store from the collection
                    //
                    if (taxonomySession.TermStores.Count > 0)
                    {
                        termStore = taxonomySession.TermStores[0];
                    }
                    else
                    {
                        throw new SPException("Unable to connect to term store for this site collection");
                    }
                }
            }
            return termStore;
        }
    }
}