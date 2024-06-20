﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.Reflection;

namespace protoc_gen_turbolink
{
    public struct GeneratedFile
    {
        public string FileName;
        public string Content;
    }
    public struct GenerateParam
	{
        public bool GenerateServiceCode;
        public bool GenerateJsonCode;
    }
    public class TurboLinkGenerator
    {
        public FileDescriptorProto ProtoFile;
        GrpcServiceFile ServiceFile;

        public List<GeneratedFile> GeneratedFiles = new List<GeneratedFile>();
        public TurboLinkGenerator(FileDescriptorProto protoFile, GrpcServiceFile serviceFile)
        {
            ProtoFile = protoFile;
            ServiceFile = serviceFile;
        }
        public void BuildOutputFiles(bool generateServiceCode, bool generateJsonCode)
        {
            GeneratedFile file;
            string turboLinkBaseName = ServiceFile.TurboLinkBasicFileName;

            GenerateParam generateParam;
            generateParam.GenerateServiceCode = generateServiceCode;
            generateParam.GenerateJsonCode = generateJsonCode;

            // xxxMarshaling.h
            Template.MarshalingH marshalingHTemplate = new Template.MarshalingH(ServiceFile, generateParam);
            file = new GeneratedFile();
            file.FileName = string.Join("/", "Private", turboLinkBaseName + "Marshaling.h");
            file.Content = marshalingHTemplate.TransformText();
            GeneratedFiles.Add(file);

            // xxxMarshaling.cpp
            Template.MarshalingCPP marshalingCPPTemplate = new Template.MarshalingCPP(ServiceFile, generateParam);
            file = new GeneratedFile();
            file.FileName = string.Join("/", "Private", turboLinkBaseName + "Marshaling.cpp");
            file.Content = marshalingCPPTemplate.TransformText();
            GeneratedFiles.Add(file);

            // xxxMessage.h
            Template.MessageH messageHTemplate = new Template.MessageH(ServiceFile, generateParam);
            file.FileName = string.Join("/", "Public", turboLinkBaseName + "Message.h");
            file.Content = messageHTemplate.TransformText();
            GeneratedFiles.Add(file);

            // xxxMessage.cpp
            Template.MessageCPP messageCPPTemplate = new Template.MessageCPP(ServiceFile, generateParam);
            file.FileName = string.Join("/", "Private", turboLinkBaseName + "Message.cpp");
            file.Content = messageCPPTemplate.TransformText();
            GeneratedFiles.Add(file);

            if (ProtoFile.Service.Count > 0 && generateParam.GenerateServiceCode)
            {
                // xxxService.h
                Template.ServiceH serviceHTemplate = new Template.ServiceH(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Public", turboLinkBaseName + "Service.h");
                file.Content = serviceHTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxClient.h
                Template.ClientH clientHTemplate = new Template.ClientH(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Public", turboLinkBaseName + "Client.h");
                file.Content = clientHTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxClient.cpp
                Template.ClientCPP clientCPPTemplate = new Template.ClientCPP(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Private", turboLinkBaseName + "Client.cpp");
                file.Content = clientCPPTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxServicePrivate.h
                Template.ServicePrivateH servicePrivateHTemplate = new Template.ServicePrivateH(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Private", turboLinkBaseName + "Service_Private.h");
                file.Content = servicePrivateHTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxServicePrivate.cpp
                Template.ServicePrivateCPP servicePrivateCPPTemplate = new Template.ServicePrivateCPP(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Private", turboLinkBaseName + "Service_Private.cpp");
                file.Content = servicePrivateCPPTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxContext.h
                Template.ContextH contextHTemplate = new Template.ContextH(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Private", turboLinkBaseName + "Context.h");
                file.Content = contextHTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxContext.cpp
                Template.ContextCPP contextCPPTemplate = new Template.ContextCPP(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Private", turboLinkBaseName + "Context.cpp");
                file.Content = contextCPPTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxService.cpp
                Template.ServiceCPP serviceCPPTemplate = new Template.ServiceCPP(ServiceFile, generateParam);
                file = new GeneratedFile();
                file.FileName = string.Join("/", "Private", turboLinkBaseName + "Service.cpp");
                file.Content = serviceCPPTemplate.TransformText();
                GeneratedFiles.Add(file);

                // xxxNode.h
                if (ServiceFile.GetTotalPingPongMethodCounts() > 0)
                {
                    Template.NodeH nodeHTemplate = new Template.NodeH(ServiceFile, generateParam);
                    file = new GeneratedFile();
                    file.FileName = string.Join("/", "Public", turboLinkBaseName + "Node.h");
                    file.Content = nodeHTemplate.TransformText();
                    GeneratedFiles.Add(file);

                    // xxxNode.cpp
                    Template.NodeCPP nodeCPPTemplate = new Template.NodeCPP(ServiceFile, generateParam);
                    file = new GeneratedFile();
                    file.FileName = string.Join("/", "Private", turboLinkBaseName + "Node.cpp");
                    file.Content = nodeCPPTemplate.TransformText();
                    GeneratedFiles.Add(file);
                }
            }
        }
    }
}
