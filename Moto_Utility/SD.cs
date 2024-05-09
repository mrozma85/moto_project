﻿namespace Moto_Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public static string SessionToken = "JWTToken";
        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}