using api_intiSoft.Models.Data;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;


namespace api_intiSoft.Data 
{
    public class TenantsDbContext : EFCoreStoreDbContext<CustomTenantInfo>
    {
        //public TenantsDbContext(DbContextOptions<TenantsDbContext> options) : base(options)

        public TenantsDbContext(DbContextOptions options) : base(options)
        {
        }
        
        

    }
}
