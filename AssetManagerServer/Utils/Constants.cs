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

            public const string InvalidUsername = "Некорректные символы в логине (можно только: a-z, A-Z, 0-9, _)";
            
            public const string InvalidPassword = "Некорректные символы в пароле (можно только: a-z, A-Z, 0-9)";
        }

        public static class AddDeleteAsset
        {
            public const string DeleteCountIsBig = "В вашем портфеле недостаточно активов подобного типа";

            public const string InvalidAssetTicker = "Некорректные символы в названии тикера";

            public const string InvalidDatetime = "Неверно задана дата (пожалуйста проверьте, что выбранная дата прошла)";
        }

        public static class Exceptions
        {
            public const string NullListGot = "Один с полученных списков оказался NULL";
        }
    }
}