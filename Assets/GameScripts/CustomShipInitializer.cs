using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.GameModels;
using MyLib.Serialization;

public class CustomShipInitializer : ShipInitializer
{
    protected override ShipInitializerModel GetInitializer()
    {
        throw new NotImplementedException();
    }
}

public class CustomShipInitializerModel : ShipInitializerModel
{
    ShipSystemModel model;
    ShipDriver driver;
    public CustomShipInitializerModel(ShipSystemModel model, ShipDriver driver)
    {
        this.model = model;
        this.driver = driver;
    }
    public CustomShipInitializerModel(string model, ShipDriver driver)
    {
        var cs = new CompactSerializer();
        this.model = cs.Deserialize<ShipSystemModel>(model);
        this.driver = driver;
    }
    protected override ShipDriver GetDriver()
    {
        return driver;
    }

    protected override ShipSystemModel GetModel()
    {
        return model;
    }
}
