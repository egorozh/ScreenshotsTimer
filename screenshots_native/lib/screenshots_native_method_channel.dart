import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';

import 'screenshots_native_platform_interface.dart';

/// An implementation of [ScreenshotsNativePlatform] that uses method channels.
class MethodChannelScreenshotsNative extends ScreenshotsNativePlatform {
  @visibleForTesting
  final methodChannel = const MethodChannel('screenshots_native');

  @override
  Future<String?> takeScreenshot() async {
    final path = await methodChannel.invokeMethod<String>('takeScreenshot');

    return path;
  }
}
