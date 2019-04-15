using System.IO;
using System.Linq;
using ESFA.DC.OPA.Model.Interface;
using ESFA.DC.OPA.Service.Interface;
using ESFA.DC.OPA.Service.Interface.Builders;
using ESFA.DC.OPA.Service.Interface.Rulebase;
using Oracle.Determinations.Engine;
using Oracle.Determinations.Masquerade.IO;

namespace ESFA.DC.OPA.Service
{
    public class OPAService : IOPAService
    {
        // Produce XDS Files.
        // Set ProduceXDS to true to produce XDS input.
        // Set XDSFolder to appropriate directory path. Result will be a set of XDS files per FundModel written to XDSFolder.
        // I am very much aware that hardcoding to the C: Drive is not service fabric aware. - LOCAL DEBUG PURPOSES ONLY.
        // File path to storage will be supplied from job context eventually.
        // I have not been authorised to spend any more time on this. AB 06/02/19.
        private const bool ProduceXDS = false;
        private const string XDSFolder = "C:\\XdsOutput\\";
        private const string LearnRefNumberAttribute = "LearnRefNumber";

        private readonly ISessionBuilder _sessionBuilder;
        private readonly IOPADataEntityBuilder _dataEntityBuilder;
        private readonly IRulebaseProvider _rulebaseProvider;

        public OPAService(ISessionBuilder sessionBuilder, IOPADataEntityBuilder dataEntityBuilder, IRulebaseProvider rulebaseProvider)
        {
            _sessionBuilder = sessionBuilder;
            _dataEntityBuilder = dataEntityBuilder;
            _rulebaseProvider = rulebaseProvider;
        }

        internal bool XDSDirectorySetup { get; set; }

        internal string XDSFilePath { get; set; }

        public IDataEntity ExecuteSession(IDataEntity globalEntity)
        {
            Session session;

            using (Stream stream = _rulebaseProvider.GetStream())
            {
                session = _sessionBuilder.CreateOPASession(stream, globalEntity);
            }

            // XDS PRE
            if (ProduceXDS)
            {
                if (!XDSDirectorySetup)
                {
                    XDSFilePath = XDSFolder + session.GetRulebase().GetBaseFileName();
                    Directory.CreateDirectory(XDSFilePath);
                    XDSDirectorySetup = true;
                }

                // Format : XDSFolder\RulebaseName\file.xds
                var xdsFile =
                    XDSFilePath
                    + "\\PRE_"
                    + globalEntity.Children
                    .Where(c => c.EntityName == "Learner")
                    .Select(g => g.Attributes.Single(k => k.Key == LearnRefNumberAttribute).Value.Value.ToString())
                    .SingleOrDefault()
                    + ".xds";

                using (FileWriter fileWriter = new FileWriter(xdsFile))
                {
                    SessionUtils.ExportSession(session, fileWriter);
                }
            }

            session.Think();

            var outputGlobalInstance = session.GetGlobalEntityInstance();
            var outputEntity = _dataEntityBuilder.CreateOPADataEntity(outputGlobalInstance, null);

            return outputEntity;
        }
    }
}
