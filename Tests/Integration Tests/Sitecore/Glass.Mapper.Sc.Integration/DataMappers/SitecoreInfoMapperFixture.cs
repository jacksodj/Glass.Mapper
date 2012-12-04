﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration;
using NUnit.Framework;
using Glass.Mapper.Sc.DataMappers;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.SecurityModel;

namespace Glass.Mapper.Sc.Integration.DataMappers
{

    [TestFixture]
    public class SitecoreInfoMapperFixture
    {
        private Database _db;

        [SetUp]
        public void Setup()
        {
            _db = Sitecore.Configuration.Factory.GetDatabase("master");
        }

        #region Method - MapToProperty

        [Test]
        [Sequential]
        public void MapToProperty_SitecoreInfoType_GetsExpectedValueFromSitecore(
            [Values(
                SitecoreInfoType.ContentPath,
                SitecoreInfoType.DisplayName,
                SitecoreInfoType.FullPath,
                SitecoreInfoType.Key,
                SitecoreInfoType.MediaUrl,
                SitecoreInfoType.Name,
                SitecoreInfoType.Path,
                SitecoreInfoType.TemplateName,
                SitecoreInfoType.Url,
                SitecoreInfoType.Version
                )] SitecoreInfoType type,
            [Values(
                "/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem", //content path
                "DataMappersEmptyItem DisplayName", //DisplayName
                "/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem", //FullPath
                "datamappersemptyitem", //Key
                "/~/media/031501A9C7F24596BD659276DA3A627A.ashx", //MediaUrl
                "DataMappersEmptyItem", //Name
                "/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem", //Path
                "DataMappersEmptyItem", //TemplateName
                "/en/sitecore/content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem.aspx", //Url
                1 //version
                )] object expected
            )
        {
            //Assign
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(config);

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        [ExpectedException(typeof(MapperException))]
        public void MapToProperty_SitecoreInfoTypeNotSet_ThrowsException()
        {
            //Assign
            SitecoreInfoType type = SitecoreInfoType.NotSet;
            
            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(config);

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            //No asserts expect exception
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeLanguage_ReturnsEn()
        {

            //Assign
            var type = SitecoreInfoType.Language;

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(config);

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");

            var expected = item.Language;


            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }
        
        [Test]
        public void MapToProperty_SitecoreInfoTypeTemplateId_ReturnsTemplateIdAsGuid()
        {
            //Assign
            var type = SitecoreInfoType.TemplateId;

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            mapper.Setup(config);

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");
            var expected = item.TemplateID.Guid;

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }

        [Test]
        public void MapToProperty_SitecoreInfoTypeTemplateId_ReturnsTemplateIdAsID()
        {
            //Assign
            var type = SitecoreInfoType.TemplateId;

            var mapper = new SitecoreInfoMapper();
            var config = new SitecoreInfoConfiguration();
            config.Type = type;
            config.PropertyInfo = typeof (Stub).GetProperty("TemplateId");

            mapper.Setup(config);

            var item = _db.GetItem("/sitecore/Content/Tests/DataMappers/SitecoreInfoMapper/DataMappersEmptyItem");
            var expected = item.TemplateID;

            Assert.IsNotNull(item, "Item is null, check in Sitecore that item exists");
            var dataContext = new SitecoreDataMappingContext(null, item);

            //Act
            var value = mapper.MapToProperty(dataContext);

            //Assert
            Assert.AreEqual(expected, value);
        }
        #endregion

        #region Stubs

        public class Stub
        {
            public ID TemplateId { get; set; }
        }

        #endregion
    }
}
