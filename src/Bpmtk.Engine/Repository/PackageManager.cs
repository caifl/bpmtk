using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bpmtk.Engine.Models;
using Bpmtk.Engine.Storage;
using Bpmtk.Engine.Utils;

namespace Bpmtk.Engine.Repository
{
    public class PackageManager : IPackageManager
    {
        protected Context context;
        protected IDbSession session;

        public PackageManager(Context context)
        {
            this.context = context;
            this.session = context.DbSession;
        }

        public virtual async Task<PackageItem> AddItemAsync(
            int packageId, 
            string name,
            string type,
            byte[] content, 
            string description)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));

            if (string.IsNullOrEmpty(type))
                throw new ArgumentException(nameof(type));

            var pkg = await this.FindAsync(packageId);
            if (pkg == null)
                throw new ObjectNotFoundException(nameof(Package));

            var item = new PackageItem();
            item.Package = pkg;
            item.Name = name;
            item.Type = type;
            item.Content = new ByteArray(content);
            item.Description = description;
            item.Created = Clock.Now;
            item.UserId = this.context.UserId;

            await this.session.SaveAsync(item);
            await this.session.FlushAsync();

            return item;
        }

        public virtual async Task CreateAsync(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            await this.session.SaveAsync(package);
            await this.session.FlushAsync();
        }

        public IPackageQuery CreateQuery()
        {
            throw new NotImplementedException();
        }

        public virtual Task<Package> FindAsync(int packageId)
            => this.session.FindAsync<Package>(packageId);

        public virtual async Task<byte[]> GetModelContentAsync(int packageId)
        {
            var query = this.session.Query<Package>()
                .Where(x => x.Id == packageId)
                .Select(x => x.Source.Value);

            return await this.session.QuerySingleAsync(query);
        }

        public virtual async Task<byte[]> GetHistoricModelContentAsync(int historicModelId)
        {
            var query = this.session.Query<HistoricModel>()
                .Where(x => x.Id == historicModelId)
                .Select(x => x.Content.Value);

            return await this.session.QuerySingleAsync(query);
        }

        public virtual async Task<byte[]> GetHistoricItemContentAsync(int historicItemId)
        {
            var query = this.session.Query<HistoricPackageItem>()
                .Where(x => x.Id == historicItemId)
                .Select(x => x.Content.Value);

            return await this.session.QuerySingleAsync(query);
        }

        public virtual async Task<IList<HistoricModel>> GetHistoricModelsAsync(int packageId)
        {
            var query = this.session.Query<HistoricModel>()
                .Where(x => x.Package.Id == packageId)
                .OrderByDescending(x => x.Id);

            return await this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<IList<HistoricPackageItem>> GetHistoricItemsAsync(int itemId)
        {
            var query = this.session.Query<HistoricPackageItem>()
                .Where(x => x.Item.Id == itemId)
                .OrderByDescending(x => x.Id);

            return await this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<IList<PackageItem>> GetItemsAsync(int packageId)
        {
            var query = this.session.Query<PackageItem>()
                .Where(x => x.Package.Id == packageId)
                .OrderByDescending(x => x.Id);

            return await this.session.QueryMultipleAsync(query);
        }

        public virtual async Task<bool> RemoveAsync(int packageId)
        {
            var package = await this.FindAsync(packageId);
            if (package == null)
                return false;

            this.session.Remove(package);
            await this.session.FlushAsync();

            return true;
        }

        public virtual async Task<bool> RemoveItemAsync(int itemId)
        {
            var item = await this.session.FindAsync<PackageItem>(itemId);
            if (item == null)
                return false;

            this.session.Remove(item);
            await this.session.FlushAsync();

            return true;
        }

        public virtual async Task<Package> SetModelContentAsync(int packageId, byte[] bpmnContent, string comment = null)
        {
            if (bpmnContent == null)
                throw new ArgumentNullException(nameof(bpmnContent));

            var query = this.session.Query<Package>();
            query = this.session.Fetch(query, x => x.Source);
            query = query.Where(x => x.Id == packageId);

            var package = await this.session.QuerySingleAsync(query);
            if (package == null)
                throw new ObjectNotFoundException(nameof(Package));

            package.Modified = Clock.Now;

            var oldContent = package.Source;

            //Assign new model.
            package.Source = new ByteArray(bpmnContent);

            //Create historic model.
            var hiModel = new HistoricModel();
            hiModel.Package = package;
            hiModel.Created = Clock.Now;
            hiModel.Comment = comment;
            hiModel.UserId = this.context.UserId;
            hiModel.Content = oldContent;

            await this.session.SaveAsync(hiModel);
            await this.session.FlushAsync();

            return package;
        }

        public virtual async Task<PackageItem> SetItemContentAsync(int itemId, byte[] content, string comment = null)
        {
            var query = this.session.Query<PackageItem>();
            query = this.session.Fetch(query, x => x.Content);
            query = query.Where(x => x.Id == itemId);

            var item = await this.session.QuerySingleAsync(query);
            if (item == null)
                throw new ObjectNotFoundException(nameof(PackageItem));

            var oldContent = item.Content;
            item.Content = new ByteArray(content);           

            var historicItem = new HistoricPackageItem();
            historicItem.Item = item;
            historicItem.Content = oldContent;
            historicItem.Created = Clock.Now;
            historicItem.Comment = comment;

            await this.session.SaveAsync(historicItem);
            await this.session.FlushAsync();

            return item;
        }
    }
}
