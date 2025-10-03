#include "include/screenshots_native/screenshots_native_plugin_c_api.h"

#include <flutter/plugin_registrar_windows.h>

#include "screenshots_native_plugin.h"

void ScreenshotsNativePluginCApiRegisterWithRegistrar(
    FlutterDesktopPluginRegistrarRef registrar) {
  screenshots_native::ScreenshotsNativePlugin::RegisterWithRegistrar(
      flutter::PluginRegistrarManager::GetInstance()
          ->GetRegistrar<flutter::PluginRegistrarWindows>(registrar));
}
