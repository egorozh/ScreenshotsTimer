import 'package:plugin_platform_interface/plugin_platform_interface.dart';

import 'screenshots_native_method_channel.dart';

abstract class ScreenshotsNativePlatform extends PlatformInterface {
  /// Constructs a ScreenshotsNativePlatform.
  ScreenshotsNativePlatform() : super(token: _token);

  static final Object _token = Object();

  static ScreenshotsNativePlatform _instance = MethodChannelScreenshotsNative();

  /// The default instance of [ScreenshotsNativePlatform] to use.
  ///
  /// Defaults to [MethodChannelScreenshotsNative].
  static ScreenshotsNativePlatform get instance => _instance;

  /// Platform-specific implementations should set this with their own
  /// platform-specific class that extends [ScreenshotsNativePlatform] when
  /// they register themselves.
  static set instance(ScreenshotsNativePlatform instance) {
    PlatformInterface.verifyToken(instance, _token);
    _instance = instance;
  }

  Future<String?> getPlatformVersion() {
    throw UnimplementedError('platformVersion() has not been implemented.');
  }
}
