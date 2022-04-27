using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityModel;

namespace Notes.Identity
{
    // содержит информацию о клиентах и ресурсах
    public class Configuration
    {
        // Scope-область представляет то, что клиентскому приложению можно использовать (идентификатор для ресурса)
        public static IEnumerable<ApiScope> ApiScopes =>
           new List<ApiScope>
           {
                new ApiScope("NotesWebAPI", "Web API")
           };
        // Id позволяет модулировать область, которая позволяет клиентскому приложению просматривать множество утверждения(Claim) о пользователе
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        // Api ресур позволяет организовать доступ ко всему защищенному ресурсу
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("NotesWebAPI", "Web API", new []
                    { JwtClaimTypes.Name})
                {
                    Scopes = {"NotesWebAPI"}
                }
            };
        // иде
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "notes-web-api",
                    ClientName = "Notes Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                   
                    RedirectUris =
                    {
                        "http://.../signin-oidc"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://..."
                    },
                    PostLogoutRedirectUris =
                    {
                        "http:/.../signout-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "NotesWebApi"
                    },

                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}
