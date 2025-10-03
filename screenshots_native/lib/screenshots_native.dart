import 'screenshots_native_platform_interface.dart';

class ScreenshotsNative {
  /// Captures everything as is shown in user's device.
  ///
  /// Returns [null] if an error ocurrs.
  /// Returns a [String] with the path of the screenshot.
  Future<String?> takeScreenshot() {
    return ScreenshotsNativePlatform.instance.takeScreenshot();
  }
}
