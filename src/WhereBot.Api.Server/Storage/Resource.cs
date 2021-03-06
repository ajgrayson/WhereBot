﻿using WhereBot.Api.Server.Services;

namespace WhereBot.Api.Server.Storage
{

    public sealed class Resource
    {

        #region Builder

        public sealed class Builder
        {

            public int Id
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public int? LocationId
            {
                get;
                set;
            }

            public Resource Build()
            {
                return new Resource
                {
                    Id = this.Id,
                    Name = this.Name,
                    LocationId = this.LocationId
                };
            }

        }

        #endregion

        #region Constructors

        public Resource()
        {
        }

        #endregion

        #region Properties

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int? LocationId
        {
            get;
            set;
        }

        #endregion

        #region Methods

        public static Resource FromModel(Models.Resource resource)
        {
            return new Resource.Builder
            {
                Id = resource.Id,
                Name = resource.Name,
                LocationId = (resource.Location == null) ? new int?() : resource.Location.Id
            }.Build();
        }

        public static Models.Resource ToModel(Resource resource, DataSet repository)
        {
            var locationService = new LocationService(repository);
            var location = resource.LocationId.HasValue ? locationService.GetLocationById(resource.LocationId.Value) : null;
            return new Models.Resource.Builder
            {
                Id = resource.Id,
                Name = resource.Name,
                Location = location
            }.Build();
        }

        public Models.Resource ToModel(DataSet repository)
        {
            return Resource.ToModel(this, repository);
        }

        #endregion

    }

}
