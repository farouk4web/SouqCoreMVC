﻿namespace Souq.Settings
{
    public static class RoleName
    {
        public const string Owners = "Owners";
        public const string Admins = "Admins";
        public const string Sellers = "Sellers";
        public const string ShippingStaff = "ShippingStaff";

        public const string OwnersAndAdmins = Owners + "," + Admins;
        public const string OwnersAndAdminsAndSellers = Owners + "," + Admins + "," + Sellers;
        public const string OwnersAndAdminsAndShippingStaff = Owners + "," + Admins + "," + ShippingStaff;
        public const string OwnersAndAdminsAndSellersAndShippingStaff = Owners + "," + Admins + "," + Sellers + "," + ShippingStaff;
    }
}