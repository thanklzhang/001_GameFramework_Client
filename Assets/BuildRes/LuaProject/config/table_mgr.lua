local table_mgr = class()

local hero_info_tb_class = require 'config/table/hero_info_tb.lua'

local all_data = { }
all_data["hero_info_tb"] =
{
    [1] =
    {
        id = 1,
        name = "zxy1"
    },
    [2] =
    {
        id = 2,
        name = "zxy2"
    },
    [3] =
    {
        id = 3,
        name = "zxy3"
    },
}

function table_mgr:init()
    print("table_mgr : init")
    self.xx = 1
end

function table_mgr:load_all_table_data()

    self.hero_info_tb = hero_info_tb_class(self)

    print("table_mgr : load finish")
end

function table_mgr:get_by_id(tb_name, id)
    return all_data[tb_name][id]
end


return table_mgr