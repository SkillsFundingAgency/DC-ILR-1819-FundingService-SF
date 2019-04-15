using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Interface;
using ESFA.DC.ILR.FundingService.Data.External.AppsEarningsHistory.Model;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Interface;
using ESFA.DC.ILR.FundingService.Data.External.LARS.Model;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Interface;
using ESFA.DC.ILR.FundingService.Data.External.Postcodes.Model;
using ESFA.DC.ILR.FundingService.Data.File.Interface;
using ESFA.DC.ILR.FundingService.FM36.Service.Constants;
using ESFA.DC.ILR.FundingService.FM36.Service.Input;
using ESFA.DC.ILR.FundingService.FM36.Service.Model;
using ESFA.DC.ILR.Tests.Model;
using ESFA.DC.OPA.Model;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.XSRC.Model.Interface.XSRCEntity;
using ESFA.DC.OPA.XSRC.Model.XSRCEntity;
using ESFA.DC.OPA.XSRC.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.ILR.FundingService.FM36.Service.Tests
{
    public class RulebaseInterfaceTests
    {
        public const string AcademicYear = "1819";
        public const string RulebaseName = "Apprenticeships Earnings Calc 18_19";
        public const string RulebaseFolder = "Rulebase";
        public const string RulebaseMasterFolder = "RulebaseMasterFiles";
        public const string XsrcName = "Inputs";

        private IList<string> xsrcEntityList = new List<string>();
        private IList<string> dataEntityEntityList = new List<string>();
        private IList<string> xsrcAttributeList = new List<string>();
        private IList<string> dataEntityAttributeList = new List<string>();
        private IDictionary<string, List<string>> xsrcEntityDictionary = new Dictionary<string, List<string>>();
        private IDictionary<string, List<string>> dataEntityDictionary = new Dictionary<string, List<string>>();

        [Fact]
        public void RulebaseVersion_AcademicYear()
        {
            var rulebaseAcademicYear = GetRulebaseVersion(RulebaseFolder).Substring(0, 4);

            rulebaseAcademicYear.Should().NotBeNull();
            rulebaseAcademicYear.Should().Be(AcademicYear);
        }

        [Fact]
        public void RulebaseVersion_AcademicYear_Match()
        {
            var rulebaseAcademicYear = GetRulebaseVersion(RulebaseFolder).Substring(0, 4);
            var masterRulebaseAcademicYear = GetRulebaseVersion(RulebaseMasterFolder).Substring(0, 4);

            masterRulebaseAcademicYear.Should().Be(rulebaseAcademicYear);
        }

        [Fact]
        public void RulebaseVersion_InterfaceVersion()
        {
            var rulebaseInterfaceVersion = GetRulebaseVersion(RulebaseFolder).Substring(5, 2);

            rulebaseInterfaceVersion.Should().NotBeNull();
        }

        [Fact]
        public void RulebaseVersion_InterfaceVersion_Match()
        {
            var rulebaseInterfaceVersion = GetRulebaseVersion(RulebaseFolder).Substring(5, 2);
            var masterRulebaseInterfaceVersion = GetRulebaseVersion(RulebaseMasterFolder).Substring(5, 2);

            masterRulebaseInterfaceVersion.Should().Be(rulebaseInterfaceVersion);
        }

        [Fact]
        public void XSRC_File_Valid()
        {
            GetXSRC(RulebaseFolder).OuterXml.ToString().Should().NotBeNull();
        }

        [Fact]
        public void XSRC_File_FilesMatch()
        {
            var xsrcFile = GetXSRC(RulebaseFolder).OuterXml.ToString();
            var masterXsrcFile = GetXSRC(RulebaseMasterFolder).OuterXml.ToString();

            masterXsrcFile.Should().BeEquivalentTo(xsrcFile);
        }

        [Fact]
        public void XSRC_File_CheckEntitiesExpected()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;

            ExpectedEntities().Should().BeEquivalentTo(GetEntityList(xsrcEntities));
        }

        [Fact]
        public void XSRC_File_CheckEntitiesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var masterXsrcEntities = GetXSRCEntity(RulebaseMasterFolder).GlobalEntity;

            GetEntityList(masterXsrcEntities).Should().BeEquivalentTo(GetEntityList(xsrcEntities));
        }

        [Fact]
        public void XSRC_File_CheckAttributesExpected()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;

            ExpectedAttributes().Should().BeEquivalentTo(GetAttributeList(xsrcEntities));
        }

        [Fact]
        public void XSRC_File_CheckAttributesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var masterXsrcEntities = GetXSRCEntity(RulebaseMasterFolder).GlobalEntity;

            GetAttributeList(xsrcEntities).Should().BeEquivalentTo(GetAttributeList(masterXsrcEntities));
        }

        [Fact]
        public void XSRC_vs_DataEntityMapper_EntitiesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var dateEntityMapperEntities = GetDataEntityMapperEntity();

            GetEntityList(dateEntityMapperEntities).Should().BeEquivalentTo(GetEntityList(xsrcEntities));
        }

        [Fact]
        public void XSRC_vs_DataEntityMapper_AttributesMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var dateEntityMapperEntities = GetDataEntityMapperEntity();

            GetAttributeList(dateEntityMapperEntities).Should().BeEquivalentTo(GetAttributeList(xsrcEntities));
        }

        [Fact]
        public void XSRC_vs_DataEntityMapper_EntityStructureMatch()
        {
            var xsrcEntities = GetXSRCEntity(RulebaseFolder).GlobalEntity;
            var dateEntityMapperEntities = GetDataEntityMapperEntity();

            var xsrcEntity = ConvertToDataEntity(xsrcEntities);

            // Get Dictionaries to compare
            var xsrcEntityAttributeDictionaries = GetXsrcEntityAttributeDictionaries(xsrcEntity);
            var dataEntityMapperAttributeDictionaries = GetDataEntityAttributeDictionaries(dateEntityMapperEntities);

            dataEntityMapperAttributeDictionaries.Should().BeEquivalentTo(xsrcEntityAttributeDictionaries);
        }

        public IList<string> ExpectedEntities()
        {
            return new List<string>
            {
                Attributes.EntityGlobal,
                Attributes.EntityLearner,
                Attributes.EntityLearningDelivery,
                Attributes.EntityApprenticeshipFinancialRecord,
                Attributes.EntityLearningDeliveryFAM,
                Attributes.EntityLearnerEmploymentStatus,
                Attributes.EntityStandardLARSApprenticshipFunding,
                Attributes.EntityFrameworkLARSApprenticshipFunding,
                Attributes.EntitySFA_PostcodeDisadvantage,
                Attributes.EntityHistoricEarningInput,
                Attributes.EntityLARSFrameworkCmnComp,
                Attributes.EntityStandardCommonComponent,
                Attributes.EntityLearningDeliveryLARS_Funding
            };
        }

        public IList<string> ExpectedAttributes()
        {
            return new List<string>
            {
                Attributes.AFinAmount,
                Attributes.AFinCode,
                Attributes.AFinDate,
                Attributes.AFinType,
                Attributes.AgreeId,
                Attributes.AimSeqNumber,
                Attributes.AimType,
                Attributes.AppProgCompletedInTheYearInput,
                Attributes.CollectionPeriod,
                Attributes.CompStatus,
                Attributes.DateEmpStatApp,
                Attributes.DateOfBirth,
                Attributes.DisApprenticeshipUplift,
                Attributes.DisUpEffectiveFrom,
                Attributes.DisUpEffectiveTo,
                Attributes.EmpId,
                Attributes.EMPStat,
                Attributes.EmpStatMon_SEM,
                Attributes.FrameworkAF1618EmployerAdditionalPayment,
                Attributes.FrameworkAF1618FrameworkUplift,
                Attributes.FrameworkAF1618ProviderAdditionalPayment,
                Attributes.FrameworkAFCareLeaverAdditionalPayment,
                Attributes.FrameworkAFEffectiveFrom,
                Attributes.FrameworkAFEffectiveTo,
                Attributes.FrameworkAFFundingCategory,
                Attributes.FrameworkAFMaxEmployerLevyCap,
                Attributes.FrameworkAFReservedValue2,
                Attributes.FrameworkAFReservedValue3,
                Attributes.FrameworkCommonComponent,
                Attributes.FworkCode,
                Attributes.AppIdentifierInput,
                Attributes.HistoricCollectionReturnInput,
                Attributes.HistoricCollectionYearInput,
                Attributes.HistoricDaysInYearInput,
                Attributes.HistoricEffectiveTNPStartDateInput,
                Attributes.HistoricEmpIdEndWithinYearInput,
                Attributes.HistoricEmpIdStartWithinYearInput,
                Attributes.HistoricFworkCodeInput,
                Attributes.HistoricLearnDelProgEarliestACT2DateInput,
                Attributes.HistoricLearner1618AtStartInput,
                Attributes.HistoricLearnRefNumberInput,
                Attributes.HistoricPMRAmountInput,
                Attributes.HistoricProgrammeStartDateIgnorePathwayInput,
                Attributes.HistoricProgrammeStartDateMatchPathwayInput,
                Attributes.HistoricProgTypeInput,
                Attributes.HistoricPwayCodeInput,
                Attributes.HistoricSTDCodeInput,
                Attributes.HistoricTNP1Input,
                Attributes.HistoricTNP2Input,
                Attributes.HistoricTNP3Input,
                Attributes.HistoricTNP4Input,
                Attributes.HistoricTotal1618UpliftPaymentsInTheYearInput,
                Attributes.HistoricTotalProgAimPaymentsInTheYearInput,
                Attributes.HistoricUKPRNInput,
                Attributes.HistoricULNInput,
                Attributes.HistoricUptoEndDateInput,
                Attributes.HistoricVirtualTNP3EndofTheYearInput,
                Attributes.HistoricVirtualTNP4EndofTheYearInput,
                Attributes.LARSFrameworkCommonComponentCode,
                Attributes.LARSFrameworkCommonComponentEffectiveFrom,
                Attributes.LARSFrameworkCommonComponentEffectiveTo,
                Attributes.LARSFundCategory,
                Attributes.LARSFundEffectiveFrom,
                Attributes.LARSFundEffectiveTo,
                Attributes.LARSFundWeightedRate,
                Attributes.LARSStandardCommonComponentCode,
                Attributes.LARSStandardCommonComponentEffectiveFrom,
                Attributes.LARSStandardCommonComponentEffectiveTo,
                Attributes.LARSVersion,
                Attributes.LearnActEndDate,
                Attributes.LearnAimRef,
                Attributes.LearnDelFAMCode,
                Attributes.LearnDelFAMDateFrom,
                Attributes.LearnDelFAMDateTo,
                Attributes.LearnDelFAMType,
                Attributes.LearnPlanEndDate,
                Attributes.LearnRefNumber,
                Attributes.LearnStartDate,
                Attributes.LrnDelFAM_EEF,
                Attributes.LrnDelFAM_LDM1,
                Attributes.LrnDelFAM_LDM2,
                Attributes.LrnDelFAM_LDM3,
                Attributes.LrnDelFAM_LDM4,
                Attributes.OrigLearnStartDate,
                Attributes.OtherFundAdj,
                Attributes.PMUKPRN,
                Attributes.PrevUKPRN,
                Attributes.PriorLearnFundAdj,
                Attributes.ProgType,
                Attributes.PwayCode,
                Attributes.StandardAF1618EmployerAdditionalPayment,
                Attributes.StandardAF1618FrameworkUplift,
                Attributes.StandardAF1618ProviderAdditionalPayment,
                Attributes.StandardAFCareLeaverAdditionalPayment,
                Attributes.StandardAFEffectiveFrom,
                Attributes.StandardAFEffectiveTo,
                Attributes.StandardAFFundingCategory,
                Attributes.StandardAFMaxEmployerLevyCap,
                Attributes.StandardAFReservedValue2,
                Attributes.StandardAFReservedValue3,
                Attributes.STDCode,
                Attributes.UKPRN,
                Attributes.ULN,
                Attributes.Year,
            };
        }

        public IList<string> GetEntityList(IXsrcEntity entity)
        {
            xsrcEntityList.Add(entity.PublicName);

            foreach (var child in entity.Children)
            {
                GetEntityList(child);
            }

            return xsrcEntityList;
        }

        public IList<string> GetEntityList(IDataEntity entity)
        {
            dataEntityEntityList.Add(entity.EntityName);

            foreach (var child in entity.Children)
            {
                GetEntityList(child);
            }

            return dataEntityEntityList;
        }

        public IList<string> GetAttributeList(IXsrcEntity entity)
        {
            foreach (var attribute in entity.Attributes)
            {
                xsrcAttributeList.Add(attribute.PublicName);
            }

            foreach (var child in entity.Children)
            {
                GetAttributeList(child);
            }

            return xsrcAttributeList;
        }

        public IList<string> GetAttributeList(IDataEntity entity)
        {
            foreach (var attribute in entity.Attributes)
            {
                dataEntityAttributeList.Add(attribute.Key);
            }

            foreach (var child in entity.Children)
            {
                GetAttributeList(child);
            }

            return dataEntityAttributeList;
        }

        public IDataEntity ConvertToDataEntity(IXsrcEntity xsrcEntity)
        {
            IDataEntity dataEntity =
                new DataEntity(xsrcEntity.PublicName)
                {
                    Attributes = xsrcEntity
                    .Attributes
                    .GroupBy(a => a.PublicName)
                    .ToDictionary(a => a.Key, b => new AttributeData(null) as IAttributeData),
                    Children = GetChidren(xsrcEntity.Children)
                };

            return dataEntity;
        }

        public IList<IDataEntity> GetChidren(IEnumerable<IXsrcEntity> xsrcEntities)
        {
            IList<IDataEntity> dataEntities = new List<IDataEntity>();

            foreach (var entity in xsrcEntities)
            {
                dataEntities.Add(new DataEntity(entity.PublicName)
                {
                    Attributes = entity
                    .Attributes
                    .GroupBy(a => a.PublicName)
                    .ToDictionary(a => a.Key, b => new AttributeData(null) as IAttributeData),
                    Children = GetChidren(entity.Children)
                });
            }

            return dataEntities;
        }

        public IDictionary<string, List<string>> GetXsrcEntityAttributeDictionaries(IDataEntity dataEntity)
        {
            xsrcEntityDictionary.Add(dataEntity.EntityName, dataEntity.Attributes.Keys.ToList());

            foreach (var child in dataEntity.Children)
            {
                GetXsrcEntityAttributeDictionaries(child);
            }

            return xsrcEntityDictionary;
        }

        public IDictionary<string, List<string>> GetDataEntityAttributeDictionaries(IDataEntity dataEntity)
        {
            dataEntityDictionary.Add(dataEntity.EntityName, dataEntity.Attributes.Keys.ToList());

            foreach (var child in dataEntity.Children)
            {
                GetDataEntityAttributeDictionaries(child);
            }

            return dataEntityDictionary;
        }

        public XsrcGlobal GetXSRCEntity(string folderName)
        {
            return new XsrcEntityBuilder(folderName + "\\XSRC\\Inputs.xsrc").BuildXsrc();
        }

        public IDataEntity GetDataEntityMapperEntity()
        {
            var larsRefererenceDataServiceMock = new Mock<ILARSReferenceDataService>();
            var postcodesReferenceDataServiceMock = new Mock<IPostcodesReferenceDataService>();
            var appsEarningsHistoryReferenceDataServiceMock = new Mock<IAppsEarningsHistoryReferenceDataService>();
            var fileDataServiceMock = new Mock<IFileDataService>();

            var learner = new TestLearner
            {
                LearnRefNumber = "Learner1",
                ULN = 1234567890,
                PostcodePrior = "Postcode",
                LearnerEmploymentStatuses = new List<TestLearnerEmploymentStatus>
                {
                    new TestLearnerEmploymentStatus
                    {
                        AgreeId = "1",
                        DateEmpStatApp = new DateTime(2018, 8, 1),
                        EmpStat = 2
                    }
                },
                LearningDeliveries = new List<TestLearningDelivery>
                {
                    new TestLearningDelivery
                    {
                        LearnAimRef = "1",
                        AimSeqNumber = 2,
                        AimType = 3,
                        CompStatus = 4,
                        FundModel = 36,
                        PwayCodeNullable = 5,
                        ProgTypeNullable = 6,
                        FworkCodeNullable = 7,
                        StdCodeNullable = 8,
                        LearnStartDate = new DateTime(2018, 8, 1),
                        LearnPlanEndDate = new DateTime(2019, 8, 1),
                        LearningDeliveryFAMs = new List<TestLearningDeliveryFAM>
                        {
                            new TestLearningDeliveryFAM()
                        },
                        AppFinRecords = new List<TestAppFinRecord>
                        {
                            new TestAppFinRecord()
                        }
                    }
                }
            };

            var larsLearningDelivery = new LARSLearningDelivery();

            var larsFunding = new List<LARSFunding>
            {
                new LARSFunding()
            };

            var learningDelivery = learner.LearningDeliveries.First();

            larsRefererenceDataServiceMock.Setup(l => l.LARSLearningDeliveryForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsLearningDelivery);
            larsRefererenceDataServiceMock.Setup(l => l.LARSFundingsForLearnAimRef(learningDelivery.LearnAimRef)).Returns(larsFunding);
            larsRefererenceDataServiceMock.Setup(l => l.LARSStandardApprenticeshipFunding(learningDelivery.StdCodeNullable, learningDelivery.ProgTypeNullable)).Returns(new List<LARSStandardApprenticeshipFunding> { new LARSStandardApprenticeshipFunding() });
            larsRefererenceDataServiceMock.Setup(l => l.LARSFrameworkApprenticeshipFunding(learningDelivery.FworkCodeNullable, learningDelivery.ProgTypeNullable, learningDelivery.PwayCodeNullable)).Returns(new List<LARSFrameworkApprenticeshipFunding> { new LARSFrameworkApprenticeshipFunding() });
            larsRefererenceDataServiceMock.Setup(l => l.LARSFrameworkCommonComponent(learningDelivery.LearnAimRef, learningDelivery.FworkCodeNullable, learningDelivery.ProgTypeNullable, learningDelivery.PwayCodeNullable)).Returns(new List<LARSFrameworkCommonComponent> { new LARSFrameworkCommonComponent() });
            larsRefererenceDataServiceMock.Setup(l => l.LARSStandardCommonComponent(learningDelivery.StdCodeNullable)).Returns(new List<LARSStandardCommonComponent> { new LARSStandardCommonComponent() });
            postcodesReferenceDataServiceMock.Setup(p => p.DASDisadvantagesForPostcode(learner.PostcodePrior)).Returns(new List<DasDisadvantage> { new DasDisadvantage() });
            appsEarningsHistoryReferenceDataServiceMock.Setup(a => a.AECEarningsHistory(learner.ULN)).Returns(new List<AECEarningsHistory> { new AECEarningsHistory() });

            return new DataEntityMapper(
                larsRefererenceDataServiceMock.Object,
                postcodesReferenceDataServiceMock.Object,
                appsEarningsHistoryReferenceDataServiceMock.Object,
                fileDataServiceMock.Object).BuildGlobalDataEntity(learner, new Global());
        }

        public string GetRulebaseVersion(string folderName)
        {
            var zipStream = new FileStream(folderName + "//" + RulebaseName + ".zip", FileMode.Open);
            var doc = GetXmlDocumentFromZip(zipStream);
            zipStream.Close();

            XmlElement root = doc.DocumentElement;
            var nodes = root.LastChild;
            XmlNodeList elemList = doc.GetElementsByTagName("conclude");

            IDictionary<string, string> attributes = new Dictionary<string, string>();

            for (int i = 0; i < elemList.Count; i++)
            {
                var e = elemList[i];

                attributes.Add(e.Attributes["attr-id"].Value, e.InnerText);
            }

            return attributes.Where(k => k.Key == "RulebaseVersion").Select(v => v.Value).SingleOrDefault();
        }

        public XmlDocument GetXmlDocumentFromZip(FileStream stream)
        {
            ZipArchive zip = new ZipArchive(stream);

            var file = zip.Entries.First(f => f.Name == RulebaseName + ".xml").Open();

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            return doc;
        }

        public XmlDocument GetXSRC(string folderName)
        {
            var stream = new FileStream(folderName + "//XSRC//" + XsrcName + ".xsrc", FileMode.Open);
            var doc = new XmlDocument();

            doc.Load(stream);
            stream.Close();

            return doc;
        }
    }
}