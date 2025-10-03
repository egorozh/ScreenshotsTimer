#ifndef FLUTTER_PLUGIN_SCREENSHOTS_NATIVE_PLUGIN_H_
#define FLUTTER_PLUGIN_SCREENSHOTS_NATIVE_PLUGIN_H_

#include <flutter/method_channel.h>
#include <flutter/plugin_registrar_windows.h>

#include <memory>

namespace screenshots_native {

class ScreenshotsNativePlugin : public flutter::Plugin {
 public:
  static void RegisterWithRegistrar(flutter::PluginRegistrarWindows *registrar);

  ScreenshotsNativePlugin();

  virtual ~ScreenshotsNativePlugin();

  // Disallow copy and assign.
  ScreenshotsNativePlugin(const ScreenshotsNativePlugin&) = delete;
  ScreenshotsNativePlugin& operator=(const ScreenshotsNativePlugin&) = delete;

  // Called when a method is called on this plugin's channel from Dart.
  void HandleMethodCall(
      const flutter::MethodCall<flutter::EncodableValue> &method_call,
      std::unique_ptr<flutter::MethodResult<flutter::EncodableValue>> result);
};

}  // namespace screenshots_native

#endif  // FLUTTER_PLUGIN_SCREENSHOTS_NATIVE_PLUGIN_H_
