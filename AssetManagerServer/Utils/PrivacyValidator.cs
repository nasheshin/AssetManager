using System.Linq;
using AssetManagerServer.Models;

namespace AssetManagerServer.Utils
{
    public class PrivacyValidator
    {

        public PrivacyValidator()
        {
        }

        public bool IsUserHasOperation(int operationId, int userId, DataContext database)
        {
            var operation = database.Operations.FirstOrDefault(o => o.Id ==  operationId);
            return operation != null && operation.UserId == userId;
        }
    }
}