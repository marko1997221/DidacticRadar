The goal of the application is to provide the interface of the radar to the operator. For this purpose, it should perform the following functions grouped into three categories:
a) Management
Sending UDP datagrams to the radar transceiver in order to select the radar operating mode, select the frequency channel, start and stop the radiation of electromagnetic energy and control the rotating platform.
b) Processing
Reception of UDP datagrams from the radar transceiver and separation into data originating from the transceiver (complex radar signal) and data arriving from the rotating platform (current position of the platform by azimuth and local angle). In this way, the reception and acquisition of the useful signal is carried out. The application processes a complex radar signal, which as a final product provides detections on the user interface
c) Display of data
Display of radar position and orientation. Layout of the radar pointer. Display of the current position of the platform (antenna beam) in the azimuth plane. Display of detections on the radar pointer.

The folowing images are initaly description of the aplication system, it was written on Serbian Ciriliy (as my native language).

![pocetni interfejs](https://github.com/marko1997221/DidacticRadar/assets/61901835/cc48e58b-5e7e-4729-874a-42abaeb1e3cc)

the folowing image is initial interface of the dekstop app!

![pokazivac](https://github.com/marko1997221/DidacticRadar/assets/61901835/6e428208-964f-4693-9737-569e85146faf)

Radar detections! 
![СТАЛНИОДРАЗИ](https://github.com/marko1997221/DidacticRadar/assets/61901835/b87899bc-e79a-4f76-ac9e-c9a83820188c)

Described legend.
![KonacnoAplikacija@4x](https://github.com/marko1997221/DidacticRadar/assets/61901835/ff552f3f-d408-42bd-8ce9-403996322d89)

