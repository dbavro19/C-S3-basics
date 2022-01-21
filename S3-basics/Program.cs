// See https://aka.ms/new-console-template for more information

using System;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;


Console.Write("Enter Access Key: ");
string accessKey = Console.ReadLine();

Console.Write("Enter Secret Key: ");
string secretKey = Console.ReadLine();

Console.Write("Enter Endpoint (in format http://endpoint:port): ");
string url = Console.ReadLine();

//string accessKey = "dom";
//string secretKey = "hRdqauOJiwpY6kbx782FFediQJJ89Op8T12ahK9M";


//create S3 Client config
AmazonS3Config config = new AmazonS3Config();
config.ServiceURL = url;
config.ForcePathStyle = true;

//Create S3 Client 
AmazonS3Client s3Client = new AmazonS3Client(
        accessKey,
        secretKey,
        config
        );

Console.Write("Bucket name to create and operate against: ");
string bucketName = Console.ReadLine();


var objectName = "testobject";
var objectBody = "Hello World";
var getBody = "";


//List all Buckets

Console.WriteLine("\nGetting a list of your buckets...");
var listResponse = await s3Client.ListBucketsAsync();
Console.WriteLine($"Number of buckets: {listResponse.Buckets.Count}");
foreach (S3Bucket b in listResponse.Buckets)
{
    Console.WriteLine(b.BucketName);
}
Console.WriteLine($"Result: {listResponse.HttpStatusCode.ToString()}");


Console.WriteLine("");

Console.WriteLine("Creating bucket with provided info");
//Create Bucket

try
{
    Console.WriteLine($"\nCreating bucket {bucketName}...");
    var createResponse = await s3Client.PutBucketAsync(bucketName);
    Console.WriteLine($"Result: {createResponse.HttpStatusCode.ToString()}");
}
catch (Exception e)
{
    Console.WriteLine("Caught exception when creating a bucket:");
    Console.WriteLine(e.Message);
}


//Write an Object
try
{
    PutObjectRequest putRequest = new PutObjectRequest();
    putRequest.BucketName = bucketName;
    putRequest.Key = objectName;
    putRequest.ContentType = "text/plain";
    putRequest.ContentBody = objectBody;
    Console.WriteLine($"\nWriting Object: " + objectName+ " to bucket: " + bucketName);
    var createResponse = await s3Client.PutObjectAsync(putRequest);
    Console.WriteLine($"Result: {createResponse.HttpStatusCode.ToString()}");
}
catch (Exception e)
{
    Console.WriteLine("Caught exception when creating a bucket:");
    Console.WriteLine(e.Message);
}


//Get Request for provided bucket/object

try
{
    GetObjectRequest getRequest = new GetObjectRequest();
    getRequest.Key = objectName;
    getRequest.BucketName = bucketName;

    Console.WriteLine($"\nReading Object: " + objectName + " to bucket: " + bucketName);
    var getResponse = await s3Client.GetObjectAsync(getRequest);

    Stream responseStream = getResponse.ResponseStream;
    StreamReader reader = new StreamReader(responseStream);
    string content = reader.ReadToEnd();

    Console.WriteLine(content);
    Console.WriteLine($"Result: {getResponse.HttpStatusCode.ToString()}");

}
catch (Exception e)
{
    Console.WriteLine("Caught exception when Reading object:");
    Console.WriteLine(e.Message);
}


//Clean up (add if needed later)



