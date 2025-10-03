import 'package:flutter_test/flutter_test.dart';
import 'package:screenshots_native/screenshots_native.dart';
import 'package:screenshots_native/screenshots_native_platform_interface.dart';
import 'package:screenshots_native/screenshots_native_method_channel.dart';
import 'package:plugin_platform_interface/plugin_platform_interface.dart';

class MockScreenshotsNativePlatform
    with MockPlatformInterfaceMixin
    implements ScreenshotsNativePlatform {

  @override
  Future<String?> getPlatformVersion() => Future.value('42');
}

void main() {
  final ScreenshotsNativePlatform initialPlatform = ScreenshotsNativePlatform.instance;

  test('$MethodChannelScreenshotsNative is the default instance', () {
    expect(initialPlatform, isInstanceOf<MethodChannelScreenshotsNative>());
  });

  test('getPlatformVersion', () async {
    ScreenshotsNative screenshotsNativePlugin = ScreenshotsNative();
    MockScreenshotsNativePlatform fakePlatform = MockScreenshotsNativePlatform();
    ScreenshotsNativePlatform.instance = fakePlatform;

    expect(await screenshotsNativePlugin.getPlatformVersion(), '42');
  });
}
