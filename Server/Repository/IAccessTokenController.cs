using Grpc.Core;
using UberAPI.Client.Model;
using InternalAPI;
using UberClient.Models;
namespace UberClient.Repository
{
    public interface IAccessTokenController
    {
        Task<string> GetAccessToken(string SessionToken, string ServiceId);
    }
}