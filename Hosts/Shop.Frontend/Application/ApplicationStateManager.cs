﻿using Shop.Frontend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public class ApplicationStateManager : IApplicationStateManager
    {
        private readonly IApplicationState _applicationState;

        public ApplicationStateManager(IApplicationState applicationState)
        {
            _applicationState = applicationState;
        }

        public async Task<Guid> CreateOrGetClientId()
        {
            const string key = "ClientId";

            var clientId = await _applicationState.GetItem<Guid>(key).ConfigureAwait(false);
            if (clientId == default)
            {
                clientId = Guid.NewGuid();
                await _applicationState.SetItem(key, clientId).ConfigureAwait(false);
            }

            return clientId;
        }

        private const string _jwtTokenKey = "JwtToken";

        public async Task<string> GetJwtToken()
        {
            return await _applicationState.GetItem<string>(_jwtTokenKey).ConfigureAwait(false);
        }

        public async Task SetJwtToken(string token)
        {
            await _applicationState.SetItem(_jwtTokenKey, token).ConfigureAwait(false);
        }
    }
}
