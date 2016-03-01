using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint;
using System.IO;

namespace Akumina.ListDefinition.Provision.ManagedMetadataSolution
{
    /// <summary>
    /// Provides methods to import term sets into the Managed Metadata term store.
    /// </summary>
    public class ManagedMetadataImporterLogic
    {
        #region Fields

        private TermStore termStore;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes an instance of <see cref="ManagedMetadataImporterLogic"/>.
        /// </summary>
        /// <param name="site">Target site collection.</param>
        public ManagedMetadataImporterLogic(SPSite site)
        {
            //
            // Get hold of the default term store
            //
            termStore = AkuminaTaxonomyExtensions.GetDefaultTermStore(site);
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Imports a term set into the Managed Metadata term store under the specified group.
        /// The group is created if it does not already exist.
        /// </summary>
        /// <param name="groupName">The name of the group under which the term set is imported.</param>
        /// <param name="csvContents"><see cref="TextReader"/> instance with the .csv file contents to import.</param>
        public void ImportTermSet(string groupName, TextReader csvContents)
        {
            string errorMessage;
            bool allTermsAdded;
            //
            // Find the group that we want to import to
            //
            Group group = termStore.Groups.FirstOrDefault(g => g.Name == groupName);
            if (group == null)
            {
                //
                // If the group doesn't exist, create it
                //
                group = CreateGroup(groupName);


                try
                {
                    //
                    // Get ImportManager object
                    //
                    ImportManager manager = group.TermStore.GetImportManager();
                    //
                    // Import term set from .csv
                    //
                    manager.ImportTermSet(group, csvContents, out allTermsAdded, out errorMessage);
                    //
                    // If there were any errors during import, throw exception
                    //
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        throw new SPException(errorMessage);
                    }

                    if (!allTermsAdded)
                    {
                        throw new SPException("Not all terms were imported successfully. Check the logs for more information.");
                    }
                }
                finally
                {
                    if (csvContents != null)
                    {
                        csvContents.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a group from the Managed Metadata term store. All term sets below the specified group are also deleted.
        /// </summary>
        /// <param name="groupName">The name of the group to delete.</param>
        public void DeleteGroup(string groupName)
        {
            //
            // Find the group we want to delete
            //
            Group group = termStore.Groups.FirstOrDefault(g => g.Name == groupName);
            if (group != null)
            {
                //
                // Delete all term sets from that group
                //
                TermSetCollection termSets = group.TermSets;
                foreach (TermSet termSet in termSets)
                {
                    termSet.Delete();
                }
                //
                // Save all changes to the term store
                //
                termStore.CommitAll();
                //
                // Delete the group itself
                //
                group.Delete();
                //
                // Save changes to the term store
                //
                termStore.CommitAll();
            }
        }

        /// <summary>
        /// Creates a group into the Managed Metadata term store.
        /// </summary>
        /// <param name="groupName">The name of the group to be created.</param>
        /// <returns><see cref="Group"/> object.</returns>
        public Group CreateGroup(string groupName)
        {
            Group group = termStore.CreateGroup(groupName);
            termStore.CommitAll();
            return group;
        }

        #endregion
    }
}
