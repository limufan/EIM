using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data
{
    public DataManager(CoreManager coreManager, string databaseConfigPath)
    {
        NHibernateHelper.ConfigFilePath = databaseConfigPath;
        this.ConsumerPhoneDataProvider = new ConsumerPhoneDataProvider(coreManager);

        log4net.Config.XmlConfigurator.Configure();
        this.Logger = log4net.LogManager.GetLogger("logger");
    }

    public ILog Logger { private set; get; }

    public ConsumerPhoneDataProvider ConsumerPhoneDataProvider { private set; get; }

    public void Load()
    {
        this.ConsumerPhoneDataProvider.Load(false);
    }
}
