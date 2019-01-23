/**
 * Verify signature using RSA-SHA256
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
import java.security.cert.CertificateFactory;
import java.security.cert.*;

class Verify {
    
	public static void main(String[] args) {

		try { 
			// read in data to verify
			Path dataToVerifyFilePath = Paths.get("data.zip");
			byte[] dataToVerfiy = Files.readAllBytes(dataToVerifyFilePath);  // any amoutn of bytes
			
			// read in signature
			Path signatureFilePath = Paths.get("signature.sig");
			byte[] signature = Files.readAllBytes(signatureFilePath);  // 256 bytes

			// read in certificate and extract public-key 
			Path certificateFilePath = Paths.get("certificate.der");
            FileInputStream inputStream = new FileInputStream(certificateFilePath.toFile());
			CertificateFactory fact = CertificateFactory.getInstance("X.509");
			X509Certificate cer = (X509Certificate) fact.generateCertificate(inputStream);
			PublicKey publicKey = cer.getPublicKey();

			// "SHA256withRSA" implements the PKCS#1 v1.5 padding and modular exponentiation with the formal name RSASSA-PKCS1-v1_5 
			// after calculating the hash over the data using SHA256.
			Signature rsaAlgo = Signature.getInstance("SHA256WithRSA"); 			
			rsaAlgo.initVerify(publicKey); 
			rsaAlgo.update(dataToVerfiy); 
			boolean isValid = rsaAlgo.verify(signature);

			System.out.println("Signature Is valid : " + isValid); 
        } 
        catch (Exception e) { 
  
            System.out.println("Exception thrown : " + e); 
        } 
    }
}