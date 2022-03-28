using System;
namespace DO
{
    public enum WeightCategories
    {
        Light = 1,
        Medium,
        Heavy
    }

    public enum DroneStatuses
    {
        Available ,
        Maintenance,
        Shipping
    }

    public enum Priorities
    {
        Standart = 1,
        Fast,
        Emergency
    }
    public enum Severity
    {
        Mild = 1,
        Severe,
        Terrible
    }
    /// <summary>
    /// permit of user/manager
    /// </summary>
    public enum Permit 
    { 
        Admin, 
        User 
    }
    /// <summary>
    /// activity of entity
    /// </summary>
    public enum Activity 
    { 
        On, 
        Off 
    }
}
