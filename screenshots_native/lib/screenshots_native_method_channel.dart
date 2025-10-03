import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';

import 'screenshots_native_platform_interface.dart';

/// An implementation of [ScreenshotsNativePlatform] that uses method channels.
class MethodChannelScreenshotsNative extends ScreenshotsNativePlatform {
  /// The method channel used to interact with the native platform.
  @visibleForTesting
  final methodChannel = const MethodChannel('screenshots_native');

  @override
  Future<String?> getPlatformVersion() async {
    final version = await methodChannel.invokeMethod<String>('getPlatformVersion');
    return version;
  }
}
