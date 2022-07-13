using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.WebAdmin.Enums;
using Talent.WebAdmin.Models;

namespace Talent.WebAdmin.Services
{
    public class RedisStoreSrevice : ITicketStore
    {
        private readonly IDistributedCache DistributedCacheMan;

        public RedisStoreSrevice(IDistributedCache dcm)
        {
            this.DistributedCacheMan = dcm;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            //var key = Guid.NewGuid().ToString();
            var key = ITalentAuthenticationSchemes.Cookie + Guid.NewGuid().ToString();

            await RenewAsync(key, ticket);
            return key;
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new DistributedCacheEntryOptions();
            //buat exipred date
            var expiresTime = DateTime.UtcNow.AddHours(12);
            options.SetAbsoluteExpiration(expiresTime);

            var value = SerializeToBytes(ticket);
            DistributedCacheMan.Set(key, value, options);
            return Task.FromResult(0);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            AuthenticationTicket ticket;
            byte[] bytes = null;
            bytes = DistributedCacheMan.Get(key);
            ticket = DeserializeFromBytes(bytes);
            return Task.FromResult(ticket);
        }

        public Task RemoveAsync(string key)
        {
            DistributedCacheMan.Remove(key);
            return Task.FromResult(0);
        }

        private static byte[] SerializeToBytes(AuthenticationTicket source)
        {
            return TicketSerializer.Default.Serialize(source);
        }

        private static AuthenticationTicket DeserializeFromBytes(byte[] source)
        {
            return source == null ? null : TicketSerializer.Default.Deserialize(source);
        }

        
    }
}
