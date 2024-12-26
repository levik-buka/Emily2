# Emily 2

## Description

Yet another application for building family tree and keeping information about relatives in logically combined. The thing here that all information is keeped safely on local machine and collaboration with relatives made by merge requests via P2P connections. So no GDPR information is collected or leaked to FAANG or government organizations.

## Features
- All data saved to local machine only
- Common used data formats: JSON, JPEG, TXT etc. Easy to view and backup without Emily application.
- Collaboration via ActivityPub decentralized social networking protocol (https://www.w3.org/TR/activitypub/#:~:text=The%20ActivityPub%20protocol%20is%20a,for%20delivering%20notifications%20and%20content)
- Multi-platform client (Avalonia UI)

## Data structure

    ├── family base path
    │   ├── surname firstname id
    │   │   ├── surname firstname id.json
    │   │   ├── surname firstname id portrait.jpg
    │   │   ├── publicity.json
    │   │   ├── documents
    │   │   ├── photos
    │   ├── surname relations.json
    │   ├── collaboration.json

### Relations
Family relations are collected in "surname relation.json" file. Supported relations are:
- biologic parents vs biologic children
- reserve parents (God parents)
- adopt parents vs adopted children **
- marriage, cohabitation **

In addition file contains version information for upgrading and merging processes.

### Personal cards
Personal data is collected to "surname firstname id.json" file:
- id
- surname, first names **
- gender **
- birthdate and place
- date of death and place
- religion, baptizing **
- educations **
- occupations **
- work places **
- links to social networks **
- residence places **
- contact information: email, phone **
- registration id
- card version
- card version timestamp

** attribute has start and end dates. Date can be full DAY.MONTH.YEAR or partly MONTH.YEAR or just YEAR. End date has also ended boolean attribure if even year is unknown.

## Collaboration
Collaboration requires registration. To collaborate you have to be member of the family and you registration is attached to one of personal cards in the relation tree.

To merge two family trees there should be at least one common personal card in both relation trees. User sends merge request to other user to merge two relation trees. Other user can accept or reject merging. Collaboration information is saved to collaboration.json file:
- name of collaborated relation tree
- name of registered user of callaborated relation tree
- list of common personal cards
- common publicity information

After relation trees are merged any changes to trees are automatically received on application start or manually on user request. User has always right to reject changes.

All data in personal card of over 70 years ago dead person is send in merge process. For other persons data sending user can define level of public information field by field. The level of publicity is common for all cards, but can be overwritten for each card separately in publicity.json file.

## Links
- https://www.sukupolku.fi/linkkeja/sukututkimusohjelmia/
- https://gramps-project.org/ - open source project
- https://www.familysearch.org/
- https://www.churchofjesuschrist.org/topics/family-history/my-family-history/learn-to-use-family-search?lang=rus