using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InVanWebApp.DAL;

namespace InVanWebApp.Repository
{
    public interface ILocationRepository
    {
        //Define function for fetching details of Location.
        IEnumerable<LocationMaster> GetAll();

        //Define function for fetching details of location master by ID.
        LocationMaster GetById(int Location_ID);

        //Function define for: Insert record.
        void Insert(LocationMaster locationMaster);

        //Function define for: Update master record.
        void Udate(LocationMaster locationMaster);

        //Function define for: Delete record of location using it's ID
        void Delete(int Location_Id);

        //Function for fetching list of country.
        IEnumerable<CountryMaster> GetCountryForDropDown();

        //Function for fetching list of state.
        IEnumerable<StateMaster> GetStateForDropdown(int CountryID);
        
        //Function for fetching list of city.
        IEnumerable<CityMaster> GetCityForDropdown(int StateID);
    }
}
