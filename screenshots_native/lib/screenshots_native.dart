
import 'screenshots_native_platform_interface.dart';

class ScreenshotsNative {
  Future<String?> getPlatformVersion() {
    return ScreenshotsNativePlatform.instance.getPlatformVersion();
  }
}
