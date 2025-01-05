Feature:
As potential candidate
I want to see the list of job offers
So that I can apply for a job

    @E2E
    Scenario: Job offers are listed
        Given Following offer are published:
          | CompanyId                            | OfferSummary        | JobDescription                                                                    | RemoteWork |
          | A62C048C-8E0F-41E2-84D4-BD061F9DDE97 | Fullstack Developer | We are looking for a fullstack developer with experience in Angular and .NET Core | Remote     |
          | 0692183B-CE56-432D-88B5-B59280A678C5 | Frontend Developer  | We are looking for a frontend developer with experience in Angular                | No         |
        When I check the job offers list
        Then the job offers should be listed