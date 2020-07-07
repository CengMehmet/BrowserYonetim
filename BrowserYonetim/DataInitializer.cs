using System.Data.Entity;

namespace BrowserYonetim
{
    internal class DataInitializer : DropCreateDatabaseIfModelChanges<BrowserContext>
    {
        protected override void Seed(BrowserContext context)
        {
            base.Seed(context);
        }
    }
}