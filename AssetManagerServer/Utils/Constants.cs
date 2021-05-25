namespace AssetManagerServer.Utils
{
    public static class Constants
    {
        public static class Sign
        {
            public const string WrongUsernameOrPassword = "Неправильный логин или пароль";
        }

        public static class Register
        {
            public const string WrongRepeatPassword = "Пароли не совпадают";
            
            public const string UsernameNotAvailable = "Данное имя пользователя занято";
        }

        public static class AddDeleteAsset
        {
            public const string DeleteCountIsBig = "В вашем портфеле недостаточно активов подобного типа";
        }
    }
}