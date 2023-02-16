require('require/global_class.lua')

client = {}

client.table_mgr = table_mgr_class()
client.ui_mgr = { };
client.ctrl_mgr = { }
client.game_data_mgr = { }
client.net_handle_mgr = { };
client.res_mgr = res_mgr_class()

client.battle_entity_mgr = { };
client.battle_skill_eft_mgr = { }
client.plot_mgr = { }

function init()
    --client.table_mgr:load_all_table_data();
    print("lua init finish")
end

function start()
    print("lua start ...  version : " .. " 1.0")

--    local hero = client.table_mgr.hero_info_tb:get_by_id(2);
--    print(hero.name)

--    print("lua start ...  version : " .. " 1.1")

end

function main()
    init()
    start()
end


main()