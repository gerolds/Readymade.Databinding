using System;

namespace Readymade.Databinding {
    public static class RemapHelper {
        public static double InverseLerpUnclamped ( float a, float b, float value ) =>
            ( value - a ) / ( b - a );

        public static double InverseLerpUnclamped ( long a, long b, long value ) =>
            ( value - a ) / ( b - a );

        public static double InverseLerpUnclamped ( double a, double b, double value ) =>
            ( value - a ) / ( b - a );

        public static double InverseLerpUnclamped ( int a, int b, int value ) =>
            ( value - a ) / ( b - a );

        public static float RemapUnclamped ( float fromMin, float fromMax, float toMin, float toMax, float value ) {
            double t = InverseLerpUnclamped ( fromMin, fromMax, value );
            return Lerp ( toMin, toMax, t );
        }

        public static double RemapUnclamped ( double fromMin, double fromMax, double toMin, double toMax, double value ) {
            double t = InverseLerpUnclamped ( fromMin, fromMax, value );
            return Lerp ( toMin, toMax, t );
        }

        public static int RemapUnclamped ( int fromMin, int fromMax, int toMin, int toMax, int value ) {
            double t = InverseLerpUnclamped ( fromMin, fromMax, value );
            return Lerp ( toMin, toMax, t );
        }

        public static long RemapUnclamped ( long fromMin, long fromMax, long toMin, long toMax, long value ) {
            double t = InverseLerpUnclamped ( fromMin, fromMax, value );
            return Lerp ( toMin, toMax, t );
        }

        public static double Lerp ( double a, double b, double t ) =>
            a + ( b - a ) * Math.Max ( 0, Math.Min ( t, 1 ) );

        public static int Lerp ( int a, int b, double t ) =>
            ( int ) ( a + ( b - a ) * Math.Max ( 0, Math.Min ( t, 1 ) ) );

        public static long Lerp ( long a, long b, double t ) =>
            ( long ) ( a + ( b - a ) * Math.Max ( 0, Math.Min ( t, 1 ) ) );

        public static float Lerp ( float a, float b, double t ) =>
            ( float ) ( a + ( b - a ) * Math.Max ( 0, Math.Min ( t, 1 ) ) );

        public static float Remap ( RemapSettings settings, float value ) =>
            Remap ( settings.FromMin, settings.FromMax, settings.ToMin, settings.ToMax, value );
        
        public static int Remap ( RemapSettingsInt settings, int value ) =>
            Remap ( settings.FromMin, settings.FromMax, settings.ToMin, settings.ToMax, value );
        
        public static long Remap ( RemapSettingsLong settings, long value ) =>
            Remap ( settings.FromMin, settings.FromMax, settings.ToMin, settings.ToMax, value );

        public static float Remap ( float fromMin, float fromMax, float toMin, float toMax, float value ) {
            float clampedValue = Math.Clamp ( value, fromMin, fromMax );
            return RemapUnclamped ( fromMin, fromMax, toMin, toMax, clampedValue );
        }

        public static double Remap ( double fromMin, double fromMax, double toMin, double toMax, double value ) {
            double clampedValue = Math.Clamp ( value, fromMin, fromMax );
            return RemapUnclamped ( fromMin, fromMax, toMin, toMax, clampedValue );
        }

        public static long Remap ( long fromMin, long fromMax, long toMin, long toMax, long value ) {
            long clampedValue = Math.Clamp ( value, fromMin, fromMax );
            return RemapUnclamped ( fromMin, fromMax, toMin, toMax, clampedValue );
        }

        public static int Remap ( int fromMin, int fromMax, int toMin, int toMax, int value ) {
            int clampedValue = Math.Clamp ( value, fromMin, fromMax );
            return RemapUnclamped ( fromMin, fromMax, toMin, toMax, clampedValue );
        }

        public static float RemapUnclamped ( RemapSettings settings, float value ) =>
            RemapUnclamped ( settings.FromMin, settings.FromMax, settings.ToMin, settings.ToMax, value );
        
        public static int RemapUnclamped ( RemapSettingsInt settings, int value ) =>
            RemapUnclamped ( settings.FromMin, settings.FromMax, settings.ToMin, settings.ToMax, value );
        
        public static long RemapUnclamped ( RemapSettingsLong settings, long value ) =>
            RemapUnclamped ( settings.FromMin, settings.FromMax, settings.ToMin, settings.ToMax, value );

        public static double Saturate ( double value ) =>
            Math.Max ( 0.0f, Math.Min ( 1, value ) );

        public static float Saturate ( float value ) =>
            Math.Max ( 0.0f, Math.Min ( 1, value ) );
    }
}