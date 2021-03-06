﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Nancy;

namespace WhereBot.Api.Server.Modules
{

    public class MappingModule : NancyModule
    {

        public MappingModule() : base("/map")
        {

            Get["/fromIds"] = parameters =>
            {
                // get the locations to render
                var locationString = (string)Request.Query["locationIds"];
                var locationIds = (string.IsNullOrEmpty(locationString)) ? new List<int>() : locationString.Split(',').Select(l => int.Parse(l)).ToList();
                var locations = this.Repository.GetLocations().Where(l => locationIds.Contains(l.Id)).ToList();
                // get the resources to render
                var resourceString = (string)Request.Query["resourceIds"];
                var resourceIds = (string.IsNullOrEmpty(resourceString)) ? new List<int>() : resourceString.Split(',').Select(l => int.Parse(l)).ToList();
                var resources = this.Repository.GetResources().Where(r => resourceIds.Contains(r.Id)).ToList();
                // render the map
                var filename = "..\\..\\App_Data\\Maps\\map-0.png";
                using (var image = Bitmap.FromFile(filename))
                {
                    using (var graphics = Graphics.FromImage(image))
                    {
                        var font = new Font("Arial", 10);
                        foreach (var location in locations)
                        {
                            Renderer.AddLocation(graphics, location, Brushes.White, Pens.Black, font, Brushes.Blue);
                        }
                        foreach (var resource in resources)
                        {
                            Renderer.AddResource(graphics, resource, Brushes.Red, Pens.Black, font, Brushes.Blue);
                        }
                    }
                    var imageFormat = ImageFormat.Png;
                    var contentType = "image/png";
                    var stream = new MemoryStream();
                    image.Save(stream, imageFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return Response.FromStream(stream, contentType);
                }
            };

            //Get["/locations/all"] = parameters =>
            //{
            //    var locations = this.Repository.GetLocations().ToList();
            //    // render the map
            //    var filename = "..\\..\\App_Data\\Maps\\map-0.png";
            //    using (var image = Bitmap.FromFile(filename))
            //    {
            //        using (var graphics = Graphics.FromImage(image))
            //        {
            //            var font = new Font("Arial", 10);
            //            foreach (var location in locations)
            //            {
            //                Renderer.AddLocation(graphics, location, Brushes.White, Pens.Black, font, Brushes.Blue);
            //            }
            //        }
            //        var imageFormat = ImageFormat.Png;
            //        var contentType = "image/png";
            //        var stream = new MemoryStream();
            //        image.Save(stream, imageFormat);
            //        stream.Seek(0, SeekOrigin.Begin);
            //        return Response.FromStream(stream, contentType);
            //    }
            //};

            //Get["/resources/nearResourceId/{resourceId}"] = parameters =>
            //{
            //    var resourceRepository = new ResourceService(this.Repository);
            //    var resourceId = int.Parse((string)parameters.resourceId);
            //    var searchRadius = int.Parse((string)this.Request.Query["searchRadius"]);
            //    // find the reference location
            //    var referenceResource = resourceRepository.GetResourceById(resourceId);
            //    var referenceLocation = referenceResource.Location;
            //    // find nearby resources
            //    var nearbyResources = resourceRepository.GetResourcesNearLocationId(referenceLocation.Id, searchRadius);
            //    // render the map
            //    var filename = var filename = "..\\..\\App_Data\\Maps\\map-0.png";
            //    using (var image = Bitmap.FromFile(filename))
            //    {
            //        using (var graphics = Graphics.FromImage(image))
            //        {
            //            Renderer.AddResource(graphics, referenceResource, Brushes.Red);
            //            foreach (var resource in nearbyResources)
            //            {
            //                Renderer.AddResource(graphics, resource, Brushes.Blue);
            //            }
            //        }
            //        var imageFormat = ImageFormat.Png;
            //        var contentType = "image/png";
            //        var stream = new MemoryStream();
            //        image.Save(stream, imageFormat);
            //        stream.Seek(0, SeekOrigin.Begin);
            //        return Response.FromStream(stream, contentType);
            //    }
            //};

        }

        #region Properties

        private DataSet Repository
        {
            get
            {
                return Globals.Repository;
            }
        }

        #endregion

    }

}
