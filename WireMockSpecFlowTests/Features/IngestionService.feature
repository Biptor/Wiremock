Feature: IngestionService
Ingestion Service for ConnectWise

@Ingestion
Scenario: Simple Tickets from ConnectWise to Watson Discovery
	Given A simple ticket comming from the Tickets API of ConnectWise
    And Watson Discovery is able to receive the ticket
    When Run the ingestion service for ConnectWise with service board
    Then Watson Discovery ingests the simple ticket with time entries
