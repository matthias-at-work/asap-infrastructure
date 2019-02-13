/**
 * Decrypt byte-array using AES-CBC-256
 */

import java.util.*; 
import java.util.Base64;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.nio.file.Files;
 
import java.nio.file.Path;
import java.nio.file.Paths;

import java.io.*;
import java.nio.*;
import java.security.*; 
import java.security.spec.*;

import javax.crypto.Cipher;
import javax.crypto.spec.SecretKeySpec;
import javax.crypto.spec.IvParameterSpec;

class Decrypt {
    
	public static void main(String[] args) {

		try {
	
			Path encryptedDataFilePath = Paths.get("data.zip.enc");
			byte[] encryptedBytes = Files.readAllBytes(encryptedDataFilePath); 

			// iv-size: 16 bytes (128 bits)
			// iv is not secret (but unique). Will be prepended to the encrypted data... 
			byte[] iv = new byte[] {
				(byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01, (byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01,
				(byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01, (byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01
			};
            IvParameterSpec ivParameterSpec = new IvParameterSpec(iv);

			// key-size: 32 bytes (256 bits)
			byte[] sharedKey = new byte[] {
				(byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01, (byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01,
				(byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01, (byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01,
				(byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01, (byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01,
				(byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01, (byte) 0x01,(byte) 0x01, (byte) 0x01, (byte) 0x01
			};
			SecretKeySpec secretKeySpec = new SecretKeySpec(sharedKey, "AES");

			// algo to use: AES CBC 256
			Cipher cipher = Cipher.getInstance("AES/CBC/PKCS7Padding");
			cipher.init(Cipher.DECRYPT_MODE, secretKeySpec, ivParameterSpec);
			byte[] decryptedBytes = cipher.doFinal(encryptedBytes);

			// write decrypted bytes to file.
			Path decryptedDataFilePath = Paths.get("data.zip");
			Files.write(decryptedDataFilePath, decryptedBytes);

			System.out.println("Wrote decrypted file to data.zip successfully.");
        } 
        catch (NoSuchAlgorithmException e) { 
  
            System.out.println("Exception thrown : " + e); 
        } 
        catch (ProviderException e) { 
  
            System.out.println("Exception thrown : " + e); 
        } 
        catch (Exception e) { 
  
            System.out.println("Exception thrown : " + e); 
        } 
    }
}