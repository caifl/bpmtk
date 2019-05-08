using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Repository;

namespace Bpmtk.Engine
{
    public interface IPackageManager
    {
        IPackageQuery CreateQuery();

        /// <summary>
        /// Create new package.
        /// </summary>
        Task CreateAsync(Package package);

        /// <summary>
        /// Fetch all items of the specified package.
        /// </summary>
        Task<IList<PackageItem>> GetItemsAsync(int packageId);

        /// <summary>
        /// Add new item for the specified package.
        /// </summary>
        /// <param name="packageId">the specified package identifier</param>
        /// <param name="content">content of item</param>
        /// <param name="description">description of item</param>
        /// <returns>The added PackageItem object</returns>
        Task<PackageItem> AddItemAsync(
            int packageId,
            string name,
            string type,
            byte[] content,
            string description);

        /// <summary>
        /// Remove the specified package item.
        /// </summary>
        /// <param name="itemId">package item identifier</param>
        /// <returns>true if item exits and removed</returns>
        Task<bool> RemoveItemAsync(int itemId);

        Task<PackageItem> SetItemContentAsync(int itemId, 
            byte[] content, 
            string comment = null);

        /// <summary>
        /// Fetch all historic models of the specified package.
        /// </summary>
        Task<IList<HistoricModel>> GetHistoricModelsAsync(int packageId);

        /// <summary>
        /// Fetch all history of the specified package item.
        /// </summary>
        /// <param name="itemId">identifier of package item</param>
        Task<IList<HistoricPackageItem>> GetHistoricItemsAsync(int itemId);

        /// <summary>
        /// Find the specified package by id.
        /// </summary>
        Task<Package> FindAsync(int packageId);

        /// <summary>
        /// Remove the specified package.
        /// </summary>
        Task<bool> RemoveAsync(int packageId);

        /// <summary>
        /// Gets the package bpmn model content.
        /// </summary>
        /// <param name="packageId">identifier of package</param>
        /// <returns>binary bpmn content</returns>
        Task<byte[]> GetModelContentAsync(int packageId);

        /// <summary>
        /// Gets the historic bpmn model content.
        /// </summary>
        /// <param name="historicModelId">identifier of historic model</param>
        /// <returns>binary bpmn content</returns>
        Task<byte[]> GetHistoricModelContentAsync(int historicModelId);

        /// <summary>
        /// Gets the historic item content.
        /// </summary>
        /// <param name="historicItemId">identifier of historic item</param>
        /// <returns>binary bpmn content</returns>
        Task<byte[]> GetHistoricItemContentAsync(int historicItemId);

        /// <summary>
        /// Set the specified package BPMN Content.
        /// </summary>
        Task<Package> SetModelContentAsync(int packageId, 
            byte[] bpmnContent,
            string comment = null);
    }
}
