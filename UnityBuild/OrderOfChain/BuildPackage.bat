::python ./BuildPackage.py %product_name% %version% %channel% %platform% %res_version% %is_build_package% %is_build_ab% %pre_compile_defines%

::IS_LOCAL_BATTLE;

python ./BuildPackage.py OrderOfChain 1.0 channel Windows 1.0 true true IS_USE_AB;IS_USE_INTERNAL_AB

pause