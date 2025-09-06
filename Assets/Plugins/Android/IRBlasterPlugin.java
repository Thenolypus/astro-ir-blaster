package com.astroremote.irblaster;

import android.content.Context;
import android.hardware.ConsumerIrManager;
import android.os.Build;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

public class IRBlasterPlugin {
    private static final String TAG = "IRBlasterPlugin";
    private static ConsumerIrManager mIrManager;

    // Initialize the IR Blaster
    public static boolean init() {
        try {
            Context context = UnityPlayer.currentActivity;
            mIrManager = (ConsumerIrManager) context.getSystemService(Context.CONSUMER_IR_SERVICE);
            
            // Check if the device has an IR blaster
            if (mIrManager == null || !mIrManager.hasIrEmitter()) {
                Log.e(TAG, "No IR emitter found on this device");
                return false;
            }
            
            Log.d(TAG, "IR emitter initialized successfully");
            return true;
        } catch (Exception e) {
            Log.e(TAG, "Error initializing IR emitter: " + e.getMessage());
            return false;
        }
    }

    // Check if the device has an IR blaster
    public static boolean hasIrEmitter() {
        try {
            if (mIrManager == null) {
                init();
            }
            
            return mIrManager != null && mIrManager.hasIrEmitter();
        } catch (Exception e) {
            Log.e(TAG, "Error checking IR emitter: " + e.getMessage());
            return false;
        }
    }

    // Send Nikon trigger command - implements the protocol we found in our research
    public static boolean sendNikonTrigger() {
        if (mIrManager == null) {
            init();
        }
        
        if (mIrManager == null || !mIrManager.hasIrEmitter()) {
            Log.e(TAG, "No IR emitter available");
            return false;
        }
        
        try {
            // Nikon pattern from our research
            // We need to convert our timing pattern to a pattern of on/off durations in microseconds
            int carrierFrequency = 38400; // 38.4KHz
            
            // The Nikon protocol we found in our research:
            // ON(2000) - OFF(27850) - ON(400) - OFF(1580) - ON(400) - OFF(3580) - ON(400) - OFF(63200)
            // Then repeat the same pattern
            int[] pattern = {
                // First sequence
                2000, 27850, 400, 1580, 400, 3580, 400, 63200,
                // Repeat sequence 
                2000, 27850, 400, 1580, 400, 3580, 400, 63200
            };
            
            // Transmit the pattern
            mIrManager.transmit(carrierFrequency, pattern);
            Log.d(TAG, "IR signal transmitted successfully");
            return true;
        } catch (Exception e) {
            Log.e(TAG, "Error transmitting IR signal: " + e.getMessage());
            return false;
        }
    }
    
    // Additional method for taking multiple shots with delay
    public static boolean takeMultipleShots(int numShots, int delayBetweenShots) {
        try {
            for (int i = 0; i < numShots; i++) {
                boolean success = sendNikonTrigger();
                if (!success) {
                    return false;
                }
                
                // Wait for the specified delay (in milliseconds)
                if (i < numShots - 1) {  // No need to delay after the last shot
                    Thread.sleep(delayBetweenShots);
                }
            }
            return true;
        } catch (InterruptedException e) {
            Log.e(TAG, "Sleep interrupted: " + e.getMessage());
            return false;
        }
    }
}