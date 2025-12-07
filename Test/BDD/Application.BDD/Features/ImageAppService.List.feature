Feature: Load Image From Private Docker Registry

Private Docker Registry Image Loading Feature

@UserAcceptanceTest
Scenario: Load All Image From Private Docker Registry
	Given Private Docker Registry Host: "https://registry.activator.com/"
	And Create a ImageListFullRequest
	When do List
	Then the result below should be returned at least:
		| Name             |
		| library/rabbitmq |
		| beats/filebeat   |